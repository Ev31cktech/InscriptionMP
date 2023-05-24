using Newtonsoft.Json.Linq;
using Inscription_Server.NetworkManagers;
using Inscription_Server.DataTypes;

namespace Inscription_Server.Scenes
{
	public class SetupScene : Scene
	{
		public ObservableList<Player> Team1 { get; protected set; } = new ObservableList<Player>();
		public ObservableList<Player> Team2 { get; protected set; } = new ObservableList<Player>();
		public JObject GameSettings { get; protected set; } = new JObject();
		public SetupScene() : base()
		{
			InitializeScene(
				new Runnable(AddPlayer),
				new Runnable(SwitchTeam),
				new Runnable(StartGame)
			);
		}
		public void AddPlayer(uint id, string name)
		{
			if (Team2.Count >= Team1.Count)
			{ TryRunAction(AddPlayer, new Player(id, name, Team.one).ToJObject()); }
			else
			{ TryRunAction(AddPlayer, new Player(id, name, Team.two).ToJObject()); }
		}
		public void AddPlayer(JObject data)
		{
			Player pData = new Player(data);
			switch (pData.Team)
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
		
	}
}
