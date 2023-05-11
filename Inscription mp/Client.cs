using Inscription_Server;
using Newtonsoft.Json.Linq;
using Inscription_Server.Exceptions;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;

namespace Inscription_mp
{
	public class Client : Inscription_Server.Client
	{
		public View CurrentView { get; private set; }

		public Client(TcpClient socket) : base(socket)
		{
		}
		public void WaitForMOTD()
		{
			DateTime endTime = DateTime.Now.AddSeconds(5);
			while (true)
			{
				if (DateTime.Now > endTime)
					throw new ClientDisconnectedException("No Response in 5 sec. Assuming disconnection");
				if (Available)
					break;
				Thread.Sleep(500);
			}
			NetworkPacket[] packets = ReadData();
			foreach (JObject i in packets[0].data) //only packet 0 should contain the MOTD and client data
			{
				JToken client = null;
				if (i.ContainsKey("Message"))
				{ Task.Run(() => MessageBox.Show(i["Message"].Value<string>())) ;}
				else if (i.TryGetValue("client", out client))
				{
					UserID = client["ID"].Value<uint>();
					IsHost = client["IsHost"].Value<bool>();
				}
				else
				{ throw new Exception("First package did not contain MOTD data. Disconnecting"); }
			}
			HandleActions(packets);
			Username = $"{App.Settings.Username}P{UserID}";
			AddData("Username",Username);
			Loop();
		}
		public override void ChangeScene(JObject data)
		{
			base.ChangeScene(data);
			CurrentView = App.GetView(CurrentScene);
			MainWindow.MainWindow_ShowView(CurrentView);
		}
	}
}
