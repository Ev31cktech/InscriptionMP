using Inscription_Server.Scenes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Inscription_Server
{
	public class Client
	{
		public bool Available { get { return socket.Available > 0; } }
		public bool IsHost { get; private set; }
		public Scene CurrentScene { get; private set; }
		public bool Connected { get; private set; } = true;
		private NetworkPacket nPacket = new NetworkPacket(0);
		private StreamWriter writer;
		private StreamReader reader;
		private TcpClient socket;
		private DateTime LastSincDTM = DateTime.Now;
		public Client(TcpClient socket, bool isHost = false)
		{
			this.socket = socket;
			IsHost = isHost;
			writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
			reader = new StreamReader(socket.GetStream());
		}

		public void AddData(JObject data)
		{
			nPacket.data.Add(data);
		}
		public void AddAction(Action<JObject> func, JObject data)
		{
			JObject action = JObject.Parse($"{{\"target\":\"{Scene.GetActionName(func)}\"}}");
			action.Add("data",data);
			nPacket.actions.Add(action);
		}

		private JArray ReadData()
		{
			JArray Jdata = new JArray();
			String rawData = "";
			while (Available)
			{
				char[] buffer = new char[1024];
				reader.Read(buffer, 0, buffer.Length);
				rawData += new String(buffer).Split('\0')[0].Trim();
			}
			foreach (String i in rawData.Split((char)3))
			{
				if (i != "")
				{ Jdata.Add(JsonConvert.DeserializeObject<JObject>(i)); }
			}
			return Jdata;
		}
		public void Loop()
		{
			if (Available)
			{
				JArray data = ReadData();
				foreach (JObject i in data)
				{
					foreach(JObject j in i["actions"])
					{
						if(j["target"].Value<string>() == "Inscription_Server.Client.ChangeScene")
						{ ChangeScene(j["data"].Value<JObject>());}
						else
						{ CurrentScene.RunAction(j["target"].Value<string>(),j["data"].Value<JObject>()); }
					}
				}
			}
			if (LastSincDTM.AddSeconds(1) < DateTime.Now && CurrentScene != null)
			{
				LastSincDTM = DateTime.Now;
				CurrentScene.RunAction(CurrentScene.Sync, CurrentScene.ToJObject());
			}
			if (nPacket.data.Count > 0)
			{
				writer.Write($"{nPacket}{(char)3}");
				nPacket = new NetworkPacket(nPacket.PacketN + 1);
			}
		}
		public virtual void ChangeScene(JObject data)
		{
			Scene scene = Scene.GetScene(data, this);
			CurrentScene = scene;
		}
		public void ChangeScene(Scene scene)
		{
			CurrentScene = Scene.GetScene(scene.ToJObject(),this);
			AddAction(ChangeScene,scene.ToJObject());
		}

		public void Shutdown()
		{
			socket.Close();
		}
	}
}
