using Newtonsoft.Json.Linq;

namespace Inscription_Server.Scenes
{
	public class BoardScene : Scene
	{
		ObservableList<CardData> team1Slots = new ObservableList<CardData>();
		ObservableList<CardData> team2Slots = new ObservableList<CardData>();
		ObservableProperty<GameStates> gamestate = new ObservableProperty<GameStates>();
		public GameRules settings { get; set; } = new GameRules();
		public BoardScene(JObject data) : base()
		{
			Sync(data,typeof(GameRules),settings);
			InitializeScene(Summon, Attack);
		}
		public void Attack(JObject data)
		{
		}
		public void Summon(JObject data)
		{

		}

		enum GameStates {
			IdleState,
			GrabCard,
			PlayerTurn,
			DealDamage,
			WinState,
		};

	}
}
