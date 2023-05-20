using Newtonsoft.Json.Linq;
using Inscription_Server.Serialization;
using Inscription_Server.NetworkManagers;

namespace Inscription_Server.Scenes
{
	public class SetupScene : Scene
	{
		public ObservableList<Player> Team1 { get; protected set; } = new ObservableList<Player>();
		public ObservableList<Player> Team2 { get; protected set; } = new ObservableList<Player>();
		public JObject GameSettings { get; protected set; }	= new JObject();
		public SetupScene() : base()
		{
			InitializeScene(AddPlayer, SwitchTeam, StartGame);
		}
		public void AddPlayer(uint id, string name)
		{
			if(Team2.Count >= Team1.Count)
			{ RunAction(null, AddPlayer, new Player(id, name, Team.one).ToJObject()); }
			else
			{ RunAction(null, AddPlayer,new Player(id, name, Team.two).ToJObject()); }
		}
		public void AddPlayer(JObject data)
		{
			Player pData = new Player(data);
			switch(pData.Team)
			{
				case Team.one:
				Team1.Add(pData);
					break;
				case Team.two:
				Team2.Add(pData);
					break;
			}
		}
		public void SwitchTeam(JObject data)
		{
			Player pData = new Player(data);
			switch (pData.Team)
			{
				case Team.one:
					Team1.Remove(pData);
					pData.Team = Team.two;
					Team2.Add(pData);
					break;
				case Team.two:
					Team2.Remove(pData);
					pData.Team = Team.one;
					Team1.Add(pData);
					break;
			}
		}
		public void StartGame(JObject data)
		{
			Server.Start_Game(this.ToJObject());
		}
		public struct Player : IToJObject
		{
			public uint UserID { get; private set; }
			public string Username { get; private set; }
			public Team Team { get; set; }
			public Player(uint userID, string username, Team team)
			{
				UserID = userID;
				Username = username;
				Team = team;
			}
			public Player(JObject data)
			{
				UserID = data.Value<uint>("UserID");
				Username = data.Value<string>("Username");
				Team = (Team)data.Value<int>("Team");
			}

			public JObject ToJObject()
			{
				return JObject.FromObject(this);
			}
		}
	}
}
