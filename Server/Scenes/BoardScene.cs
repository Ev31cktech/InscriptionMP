using Inscryption_Server.Classes;
using Inscryption_Server.DataTypes;
using Newtonsoft.Json.Linq;

namespace Inscryption_Server.Scenes
{
	internal class BoardScene : Scene<BoardScene>
	{
		public readonly Team Team;
		public ObservableList<CardData> Team1Slots { get; set; } = new ObservableList<CardData>(new CardData[4], false, false);
		public ObservableList<CardData> Team2Slots { get; set; } = new ObservableList<CardData>(new CardData[4], false, false);
		public ObservableList<CardData> Team1HandCards { get; set; } = new ObservableList<CardData>();
		public ObservableList<CardData> TeamHandCards2 { get; set; } = new ObservableList<CardData>();
		//public ObservableList<Cost> Team1Costs {get; set; } = new ObservableList<Cost>();
		//public ObservableList<Cost> Team2Costs {get; set; } = new ObservableList<Cost>();
		public ObservableProperty<GameStates> GameState { get; set; } = new ObservableProperty<GameStates>();
		public GameRules settings { get; set; } = new GameRules();
		public BoardScene() : base()
		{
			InitializeScene(
			new Runnable(thisScene.Summon),
			new Runnable(thisScene.Attack));
		}
		internal void Initialize(JToken sceneData)
		{}
		internal bool Attack(JObject data)
		{
			return false;
		}
		internal bool Summon(JObject data)
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
