using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Inscryption_Server.Exceptions;
using Inscryption_Server.Events.INotifyEvent;
using Inscryption_Server.DataTypes;

namespace Inscryption_Server
{
	public class Client
	{
		private const char ETX = (char)3;
		public uint UserID { get; protected set; }
		public bool Available { get { return socket.Available > 0; } }
		public bool IsHost { get; protected set; }
		public string Username { get; protected set; }
		public Scene CurrentScene { get; protected set; }
		public bool Connected { get; private set; } = true;

		protected StreamWriter writer;
		protected StreamReader reader;
		protected TcpClient socket;
		private NetworkPacket nPacket = new NetworkPacket(0);
		private DateTime LastSyncDTM = DateTime.Now;
		internal Client(TcpClient socket, uint iD) : this(socket)
		{
			UserID = iD;
			IsHost = UserID == 0;
			AddData("Message", "Hello, welcome to the lobby");
			AddData("client", new JObject(
						new JProperty("ID", UserID),
						new JProperty("IsHost", IsHost))
					);
			DataSend();
			DateTime endTime = DateTime.Now.AddSeconds(5);
			while (true)
			{
				if (DateTime.Now > endTime)
					throw new ClientDisconnectedException("No Response in 5 sec. Assuming disconnection");
				if (Available)
					break;
				Thread.Sleep(500);
			}
			NetworkPacket[] packets = GetPackets();
			foreach (JObject data in packets[0].data) //only packet 0 should contain the MOTD and client data
			{
				if (data.ContainsKey("Username"))
					Username = data.Value<string>("Username");
			}
		}
		internal Client(TcpClient socket)
		{
			this.socket = socket;
			writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
			reader = new StreamReader(socket.GetStream());
		}

		internal void AddData(string Name, object data)
		{
			nPacket.data.Add(new JObject(new JProperty(Name, data)));
		}

		internal void AddAction(Func<JObject, bool> func, JObject data)
		{
			JObject action = JObject.Parse($"{{\"target\":\"{Scene.GetActionName(func)}\"}}");
			action.Add("data", data);
			nPacket.actions.Add(action);
		}

		protected NetworkPacket[] GetPackets()
		{
			List<NetworkPacket> packets = new List<NetworkPacket>();

			String rawData = "";
			while (Available)
			{
				char[] buffer = new char[1024];
				reader.Read(buffer, 0, buffer.Length);
				rawData += new String(buffer).Split('\0')[0].Trim();
			}
			foreach (String i in rawData.Split(ETX))
			{
				if (i != "")
				{
					NetworkPacket packet = new NetworkPacket(JsonConvert.DeserializeObject<JObject>(i));
					packets.Add(packet);
					App.Logger.Debug($"data recieved: {packet}");
				}
			}
			return packets.ToArray();
		}
		private void RunAction(String func, JObject data)
		{
			try
			{
				if(!func.StartsWith(CurrentScene.SceneName))
					throw new Exception("NotForCurrentSceneException");
				CurrentScene.TryRunAction(func, data, this);
			}
			catch (Exception e)
			{
				App.Logger.Error(e);
			}
		}
		internal virtual void Loop()
		{
			DataRecieve();
			//if (LastSincDTM.AddSeconds(1) < DateTime.Now && CurrentScene != null)
			//{
			//	LastSincDTM = DateTime.Now;
			//	AddAction(CurrentScene.Sync, CurrentScene.ToJObject());
			//}
			DataSend();
		}
		public void HandleActions(NetworkPacket packet)
		{
			foreach (JObject i in packet.actions)
			{
				switch (i.Value<string>("target"))
				{
					case "Inscryption_Server.Client.ChangeScene":
						ChangeScene(i.Value<JObject>("data"));
						break;
					case "Inscryption_Server.Client.KickPlayer":
						App.Server.Player_Kick(i.Value<Player>("data"));
						break;
					default:
						RunAction(i["target"].Value<string>(), i.Value<JObject>("data"));
						break;
				}
			}
		}
		public void HandleData(NetworkPacket packet)
		{/*TODO fix function*/}

		protected virtual void CurrentScene_ActionRunEvent(Client sender, ActionRunEventData e)
		{
			if (sender != this && e.Executer == Runnable.Runner.Both)
				AddAction(e.Runnable.Action, e.Data);
		}
		
		public void ChangeScene(Scene scene)
		{
			CurrentScene = scene;
			AddAction(ChangeScene, scene.ToJObject());
			CurrentScene.ActionRunEvent += CurrentScene_ActionRunEvent;
		}
		public virtual bool ChangeScene(JObject data)
		{
			Scene scene = Scene.GetScene(data);
			CurrentScene = scene;
			CurrentScene.ActionRunEvent += CurrentScene_ActionRunEvent;
			return true;
		}

		public void KickPlayer(Player player)
		{
			AddAction(TransferHost, player.ToJObject());
		}
		public void TransferHost(Player player)
		{
			IsHost = false;
			AddAction(TransferHost, player.ToJObject());
		}
		public bool TransferHost(JObject data)
		{
			return true;
		}
		public void Shutdown()
		{
			socket.Close();
			Connected = false;
		}

		public void AddMessage(string message)
		{
			AddData("message", message);
		}

		public void DataSend()
		{
			if (nPacket.data.Count > 0 | nPacket.actions.Count > 0)
			{
				writer.Write($"{nPacket}{ETX}");
				App.Logger.Debug($"data send: {nPacket}");
				nPacket = new NetworkPacket(nPacket.PacketN + 1);
			}
		}

		public void DataRecieve()
		{
			if (Available)
			{
				NetworkPacket[] packets = GetPackets();
				foreach (NetworkPacket packet in packets)
				{
					HandleData(packet);
					HandleActions(packet);
				}
			}
		}
	}
}
