using Newtonsoft.Json.Linq;

namespace Inscription_Server.Scenes
{
	public class BoardScene : Scene
	{
		public readonly Team Team; 
		public ObservableList<CardData> Team1Slots { get; set; } = new ObservableList<CardData>();
		public ObservableList<CardData> Team2Slots { get; set; } = new ObservableList<CardData>();
		public ObservableProperty<GameStates> GameState { get; set; } = new ObservableProperty<GameStates>();
		public GameRules settings { get; set; } = new GameRules();
		public BoardScene(JObject data) : base()
		{
			InitializeScene(Summon, Attack);
		}
		public void Attack(JObject data)
		{}
		public void Summon(JObject data)
		{}

		public enum GameStates {
			IdleState,
			GrabCard,
			PlayerTurn,
			DealDamage,
			WinState,
		};

	}
}
