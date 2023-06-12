using Inscryption_Server.DataTypes;
using Newtonsoft.Json.Linq;

namespace Inscryption_Server.Scenes
{
	public class BoardScene : Scene
	{
		public readonly Team Team;
		public ObservableList<CardData> Team1Slots { get; set; } = new ObservableList<CardData>(new CardData[4], false, false);
		public ObservableList<CardData> Team2Slots { get; set; } = new ObservableList<CardData>(new CardData[4], false, false);
		public ObservableList<CardData> Team1HandCards { get; set; } = new ObservableList<CardData>();
		public ObservableList<CardData> TeamHandCards2 { get; set; } = new ObservableList<CardData>();
		public ObservableProperty<GameStates> GameState { get; set; } = new ObservableProperty<GameStates>();
		public GameRules settings { get; set; } = new GameRules();
		public BoardScene() : base()
		{
			InitializeScene(
				new Runnable(Summon),
				new Runnable(Attack)
			);
		}
		internal void Initialize(JToken sceneData)
		{}
		public bool Attack(JObject data)
		{
			return false;
		}
		public bool Summon(JObject data)
		{
			return false;
		}
		public bool Destroy(JObject data)
		{
			return false;
		}

		public enum GameStates
		{
			IdleState,
			GrabCard,
			PlayerTurn,
			DealDamage,
			WinState,
		};

	}
}
