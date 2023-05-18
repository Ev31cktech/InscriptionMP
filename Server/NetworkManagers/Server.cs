using Inscription_mp;
using Inscription_Server.Scenes;
using Newtonsoft.Json.Linq;
//using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using static Inscription_Server.Scenes.SetupScene;

//[assembly: Log(AttributeTargetMembers = "regex:^get_|^set_|^.?Loop", AttributeExclude = true, AttributePriority = 3)]
namespace Inscription_Server.NetworkManagers
{
	public abstract class Server
	{
		private static Server server;
		private MapGenerator mapGenerator = new MapGenerator();
		public Timer looper;
		private bool looping = false;
		public bool SetupState { get; set; } = true;
		public static bool IsServer { get => server != null; }
		public Scene CommonScene { get; private set; }
		public bool IsAllive { get { return looper != null; } }
		protected List<Client> clients = new List<Client>();

		private Player[] team1 = new Player[0];
		private Player[] team2 = new Player[0];
		public Server()
		{
			server = this;
			CommonScene = new SetupScene();
		}
		public virtual void Start()
		{
			looper = new Timer((sender) => _Loop(), null, 0, 1);
			App.Logger.Info("Server started");
		}
		public static void Start_Game(JObject data)
		{
			server.SetupState = false;
			if (server == null) //ik ben geen server
				return;
			JToken sceneData = data["sceneData"];
			//server.team1 = sceneData.Values<Player>("Team1");
			//server.team2 = sceneData.Value<Player[]>("Team2");
			//generate new map
			BoardScene scene = new BoardScene(sceneData.Value<JObject>("rules"));
			server.clients.ForEach( i => {i.ChangeScene(scene);});
		}
		public void Stop()
		{
			App.Logger.Info("Shutting down server...");
			if (looper != null)
				looper.Dispose();
			if(server != null)
			server.Shutdown();
		}

		private void _Loop()
		{
			if (!looping)
			{
				try
				{
					looping = true;
					server.Loop();

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
	}
}