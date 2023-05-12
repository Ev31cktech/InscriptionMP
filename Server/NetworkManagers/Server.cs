using Inscription_mp;
using Inscription_Server.Scenes;
using Newtonsoft.Json.Linq;
//using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading;

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
		public Scene CommonScene { get; private set; }
		public bool IsAllive { get { return looper != null; } }
		protected List<Client> clients = new List<Client>();

		private string[] team1 = new string[0];
		private string[] team2 = new string[0];
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
			server.team1 = data.Value<string[]>("team1");
			server.team2 = data.Value<string[]>("team2");
			//generate new map
			BoardScene scene = new BoardScene(new JObject());
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