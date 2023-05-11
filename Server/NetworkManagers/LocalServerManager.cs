using Inscription_Server.Scenes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Inscription_Server.NetworkManagers
{
	public class LocalServerManager : AServerManager
	{
		TcpListener tcpListener;
		List<Client> tcpClients = new List<Client>();
		private uint userID = 0;
		public LocalServerManager(IPAddress ip)
		{
			tcpListener = new TcpListener(ip, 5801);
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
			while (SetupState && tcpListener.Pending())
			{
				Client client;
				try
				{
					client = new Client(tcpListener.AcceptTcpClient(), userID++);
				}
				catch (Exception e)
				{
					Server.Logger.Error(e.Message, e);
					continue;
				}
				SetupScene setupScene = CommonScene as SetupScene;
				SetupScene.Player Pdata = new SetupScene.Player(client.UserID, client.Username, SetupScene.Team.one);
				client.ChangeScene(setupScene);
				setupScene.RunAction(client ,setupScene.AddPlayer, Pdata.ToJObject());
				tcpClients.Add(client);
			}

			for (int i = 0; i < tcpClients.Count; i++)
			{
				Client client = tcpClients[i];
				if (!client.Connected)
				{
					tcpClients.RemoveAt(i--);
				}
				client.Loop();
			}
		}
	}
}
