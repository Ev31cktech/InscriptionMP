using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Sockets;

namespace Inscription_Server
{
	public class Client
	{
		public bool Available { get { return socket.Available > 0; } }
		public bool IsHost { get; private set; }
		public Scene CurrentScene { get; private set; }
		public bool Connected { get; private set; } = true;
		private JArray nPacket;
		private StreamWriter writer;
		private StreamReader reader;
		private TcpClient socket;
		public Client(TcpClient socket, bool isHost = false)
		{
			this.socket = socket;
			IsHost = isHost;
			writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
			reader = new StreamReader(socket.GetStream());
			Add_Data(JObject.Parse("{\"Message\":\"Hello, welcome to the lobby\"}"));
		}

		public void Add_Data(JToken data)
		{
			nPacket.Add(data);
		}

		public JObject ReadData()
		{
			String rawData = "";
			while (Available)
			{
				char[] buffer = new char[1024];
				reader.ReadBlock(buffer, 0, buffer.Length);
				rawData += new String(buffer);
			}
			return JObject.Parse(rawData);
		}
		public void Loop()
		{
			if (Available)
			{
				JObject data = ReadData();
				foreach (JObject test in data.Values())
				{}
			}
		}
		public void Shutdown()
		{
			socket.Close();
		}
	}
}
