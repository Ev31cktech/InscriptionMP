using Inscryption_mp;
using Inscryption_Server.DataTypes;
using Inscryption_Server.Scenes;
using Newtonsoft.Json.Linq;
//using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading;

//[assembly: Log(AttributeTargetMembers = "regex:^get_|^set_|^.?Loop", AttributeExclude = true, AttributePriority = 3)]
namespace Inscryption_Server.NetworkManagers
{
	public abstract class Server
	{
		public static Server ThisServer { get; private set;}
		private MapGenerator mapGenerator = new MapGenerator();
		private Timer looper;
		private bool looping = false;
		private Player[] team1 = new Player[0];
		private Player[] team2 = new Player[0];
		protected List<Client> clients = new List<Client>();
		public bool SetupState { get; set; } = true;
		public static bool IsServer { get => ThisServer != null; }
		public bool IsAllive { get { return looper != null; } }
		public Scene CommonScene { get; private set; }

		public Server()
		{
			ThisServer = this;
			CommonScene = new SetupScene();
		}
		public virtual void Start()
		{
			looper = new Timer((sender) => Timer_Loop(), null, 0, 1);
			App.Logger.Info("Server started");
		}
		public void Start_Game(JObject data)
		{
			ThisServer.SetupState = false;
			if (!IsServer) //ik ben geen server
				return;
			JToken sceneData = data["sceneData"];
			//server.team1 = sceneData.Values<Player>("Team1");
			//server.team2 = sceneData.Value<Player[]>("Team2");
			//generate new map
			BoardScene scene = new BoardScene();
			scene.Initialize(sceneData);
			ThisServer.clients.ForEach( i => {i.ChangeScene(scene);});
		}
		public virtual void Stop()
		{
			App.Logger.Info("Shutting down server...");
			if (ThisServer.looper != null)
				ThisServer.looper.Dispose();
			if(ThisServer != null)
			ThisServer.Shutdown();
		}

		private void Timer_Loop()
		{
			if (!looping)
			{
				try
				{
					looping = true;
					ThisServer.Loop();

				}
				catch (Exception e)
				{
					App.Logger.Error(e.Message, e);
					throw;
				}
				finally
				{
					looping = false;
				}
			}
		}
		public abstract void Loop();
		public abstract void Shutdown();
		public void Player_Kick(Player player)
		{
			Client client = clients.Find(i => i.UserID == player.UserID);
			client.AddMessage("you have been kicked from the lobby");
			client.DataSend();
		}
	}
}