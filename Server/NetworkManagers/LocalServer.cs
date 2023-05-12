using Inscription_Server.Exceptions;
using Inscription_Server.Scenes;
using System;
using System.Net;
using System.Net.Sockets;

namespace Inscription_Server.NetworkManagers
{
	public class LocalServer : Server
	{
		TcpListener tcpListener;
		private uint userID = 0;
		public LocalServer(IPAddress ip)
		{
			tcpListener = new TcpListener(ip, 5801);
		}

		public override void Shutdown()
		{
			foreach (Client client in clients)
			{
				client.Shutdown();
			}
			tcpListener.Stop();
		}
		public override void Start()
		{
			tcpListener.Start();
			base.Start();
		}
		public override void Loop()
		{
			while (tcpListener.Pending())
			{
				if (SetupState)
				{
					Client client;
					try
					{
						client = new Client(tcpListener.AcceptTcpClient(), userID++);
					}
					catch (Exception e)
					{
						App.Logger.Error(e.Message, e);
						continue;
					}
					SetupScene setupScene = CommonScene as SetupScene;
					SetupScene.Player Pdata = new SetupScene.Player(client.UserID, client.Username, SetupScene.Team.one);
					client.ChangeScene(setupScene);
					setupScene.RunAction(client, setupScene.AddPlayer, Pdata.ToJObject());
					clients.Add(client);
				}
				else
				{
					Client client = new Client(tcpListener.AcceptTcpClient());
					client.AddData("Message","This lobby has already started");
					client.Shutdown();
				}
			}

			for (int i = 0; i < clients.Count; i++)
			{
				Client client = clients[i];
				if (!client.Connected)
				{
					clients.RemoveAt(i--);
				}
				client.Loop();
			}
		}
	}
}
