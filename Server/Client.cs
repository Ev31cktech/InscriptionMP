using System;
using System.IO;
using System.Net.Sockets;

namespace Server
{
	public class Client
	{
		public bool Available { get{return socket.Available > 0;} }
		public bool IsHost {get; private set; }
		public Scene CurrentScene {get; private set;}
		public bool Connected { get; private set; } = true;
		StreamWriter writer;
		StreamReader reader;

		TcpClient socket;
		public Client(TcpClient socket)
		{
			this.socket = socket;
			writer = new StreamWriter(socket.GetStream()) { AutoFlush = true};
			reader = new StreamReader(socket.GetStream());
		}

		internal void Loop()
		{
			String rawData = "";
			while(Available)
			{
				char[] buffer = new char[1024];
				reader.ReadBlock(buffer,0,buffer.Length);
				rawData += new String(buffer);
			}
			Console.WriteLine($"client send:{rawData}");

		}

		internal void Shutdown()
		{
			socket.Close();
		}
	}
}
