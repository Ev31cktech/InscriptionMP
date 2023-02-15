using Inscription_Server.Scenes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Inscription_Server.NetworkManagers
{
	public class LocalServerManager : AServerManager
	{
		TcpListener tcpListener;
		List<Client> tcpClients = new List<Client>();
		public LocalServerManager(IPAddress ip)
		{
			tcpListener = new TcpListener(ip,5801);
			tcpListener.Start();
		}

		public override void Shutdown()
		{
			foreach (Client client in tcpClients)
			{
				client.Shutdown();
			}
			tcpListener.Stop();
		}

		public override void Loop()
		{
			while(SetupState && tcpListener.Pending())
			{ 
				Client client = new Client(tcpListener.AcceptTcpClient());
				client.AddData(JObject.Parse("{\"Message\":\"Hello, welcome to the lobby\"}"));
				client.ChangeScene(CommonScene);
				tcpClients.Add(client);
			}
				
			for(int i = 0; i < tcpClients.Count; i++)
			{
				Client client = tcpClients[i];
				if(!client.Connected)
				{
					tcpClients.RemoveAt(i--);
				}
				client.Loop();
			}
		}
	}
}
