using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Sockets;

namespace Inscription_Server
{
	public class Client
	{
		public bool Handled { get; private set; }
		public bool Available { get { return socket.Available > 0; } }
		public bool IsHost { get; private set; }
		public Scene CurrentScene { get; private set; }
		public bool Connected { get; private set; } = true;
		private NetworkPacket nPacket = new NetworkPacket(0);
		private StreamWriter writer;
		private StreamReader reader;
		private TcpClient socket;
		public Client(TcpClient socket, bool isHost = false)
		{
			this.socket = socket;
			IsHost = isHost;
			writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
			reader = new StreamReader(socket.GetStream());
		}

		public void Add_Data(JObject data)
		{
			nPacket.data.Add(data);
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
			Handled = true;
			if (Available)
			{
				JArray data = ReadData();
				foreach (JObject i in data)
				{}
			}
			if (nPacket.data.Count > 0)
			{
				writer.Write($"{nPacket}{(char)3}");
				nPacket = new NetworkPacket(nPacket.PacketN + 1);
			}
			Handled = false;
		}
		public void Shutdown()
		{
			socket.Close();
		}
	}
}
