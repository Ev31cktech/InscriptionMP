using Newtonsoft.Json.Linq;
using Inscryption_Server.DataTypes;
using static Inscryption_Server.DataTypes.Runnable;

namespace Inscryption_Server.Scenes
{
	internal class SetupScene : Scene<SetupScene>
	{
		public ObservableList<Player> Team1 { get; protected set; } = new ObservableList<Player>();
		public ObservableList<Player> Team2 { get; protected set; } = new ObservableList<Player>();
		public JObject GameSettings { get; protected set; } = new JObject();
		public SetupScene() : base()
		{
			InitializeScene(
			new Runnable(thisScene.AddPlayer),
			new Runnable(thisScene.SwitchTeam),
			new Runnable(thisScene.StartGame, Runner.Server));
		}
		internal void AddPlayer(Client client)
		{
			if (Team2.Count >= Team1.Count)
			{ TryRunAction(AddPlayer, new Player(client.UserID, client.Username, Team.one).ToJObject()); }
			else
			{ TryRunAction(AddPlayer, new Player(client.UserID, client.Username, Team.two).ToJObject()); }
		}
		internal bool AddPlayer(JObject data)
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
			return true;
		}
		internal bool SwitchTeam(JObject data)
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
			return true;
		}
		internal bool StartGame(JObject data)
		{
			App.Server.Start_Game(ToJObject());
			return true;
		}
	}
}
