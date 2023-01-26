using Inscription_mp.Scenes.GameScenes;
using System.Collections.Generic;

namespace Inscription_mp.Scenes
{
	internal class GameScene : Scene
	{
		enum GameStates {
			WaitState,
			GrabCard,
			PlayerTurn,
			DealDamage,
			WinState,
		};
		GameRules settings;
		BoardView boardView;
		List<Card> handCards; 
		Stack<Card> cards;
		public GameScene(GameRules gameSettings, Card[] MainDeckCards) : base(new BoardView(gameSettings))
		{
			boardView = getView(0,0) as BoardView;
		}
		public void Game_Initialize()
		{

		}
	}
}