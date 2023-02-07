using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Inscription_Server.NetworkManagers
{
	public class LocalServerManager : AServerManager
	{
		bool setupState = true;
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
			while(setupState && tcpListener.Pending())
			{ 
				Client client = new Client(tcpListener.AcceptTcpClient());
				client.Add_Data(JObject.Parse("{\"Message\":\"Hello, welcome to the lobby\"}"));
				tcpClients.Add(client);
			}
				
			for(int i = 0; i < tcpClients.Count; i++)
			{
				Client client = tcpClients[i];
				if(!client.Connected)
				{
					tcpClients.RemoveAt(i--);
				}
				if(!client.Handled)
				{ client.Loop(); }
			}
		}
	}
}
