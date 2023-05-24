using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Inscription_Server.Exceptions;
using Inscription_Server.Events.INotifyEvent;
using Inscription_Server.NetworkManagers;
using Inscription_Server.DataTypes;
using static Inscription_Server.DataTypes.Runnable;

namespace Inscription_Server
{
	public class Client
	{
		public const char ETX = (char)3;
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
		public Client(TcpClient socket, uint iD) : this(socket)
		{
			UserID = iD;
			IsHost = UserID == 0;
			AddData("Message", "Hello, welcome to the lobby");
			AddData("client", new JObject(
						new JProperty("ID", UserID),
						new JProperty("IsHost", IsHost))
					);
			Loop();
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
		public Client(TcpClient socket)
		{
			this.socket = socket;
			writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
			reader = new StreamReader(socket.GetStream());
		}

		public void AddData(string Name, object data)
		{
			nPacket.data.Add(new JObject(new JProperty(Name, data)));
		}
		public void AddAction(ActionRunEventData e)
		{
			JObject action = JObject.Parse($"{{\"target\":\"{Scene.GetActionName(e.Runnable.Action)}\"}}");
			action.Add("data", e.Data);
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
		public virtual void RunAction(Client sender, String func, JObject data)
		{
			try
			{
				CurrentScene.TryRunAction(func, data, sender);
			}
			catch (Exception e)
			{
				App.Logger.Error(e);
				throw;
			}
		}
		public void Loop()
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
			//if (LastSincDTM.AddSeconds(1) < DateTime.Now && CurrentScene != null)
			//{
			//	LastSincDTM = DateTime.Now;
			//	AddAction(CurrentScene.Sync, CurrentScene.ToJObject());
			//}
			if (nPacket.data.Count > 0 | nPacket.actions.Count > 0)
			{
				writer.Write($"{nPacket}{ETX}");
				App.Logger.Debug($"data send: {nPacket}");
				nPacket = new NetworkPacket(nPacket.PacketN + 1);
			}
		}
		public void HandleActions(NetworkPacket packet)
		{
			foreach (JObject i in packet.actions)
			{
				if (i.Value<string>("target") == "Inscription_Server.Client.ChangeScene")
					ChangeScene(i["data"].Value<JObject>());
				else
					RunAction(this, i["target"].Value<string>(), i["data"].Value<JObject>());
			}
		}
		public void HandleData(NetworkPacket packet)
		{//TODO fix function

		}
		public virtual void ChangeScene(JObject data)
		{
			Scene scene = Scene.GetScene(data);
			CurrentScene = scene;
			CurrentScene.ActionRunEvent += CurrentScene_ActionRunEvent;
		}

		private void CurrentScene_ActionRunEvent(Client sender, ActionRunEventData e)
		{
			if (sender is Client)
			{
				if (e.Runnable.Executer == Runner.Client)
				{
					if (sender != this)
					{ AddAction(e); }
				}
				e.Runnable.Action.Invoke(e.Data);
			}
			else if (Server.IsServer)
			{
				if (e.Runnable.Executer == Runner.Server)
				{ AddAction(e); }
				e.Runnable.Action.Invoke(e.Data);
			}
		}

		public void ChangeScene(Scene scene)
		{
			CurrentScene = scene;
			AddAction(new ActionRunEventData(new Runnable(ChangeScene), scene.ToJObject()));
			CurrentScene.ActionRunEvent += CurrentScene_ActionRunEvent;
		}

		public void Shutdown()
		{
			socket.Close();
			Connected = false;
		}
	}
}
