using Inscription_mp.Scenes.GameScenes;
using System.Collections.Generic;
using System.Windows.Input;

namespace Inscription_mp.Scenes
{
	internal class GameScene : Scene
	{
		public GameScene(GameSettings gameSettings) : base(new BoardView(gameSettings))
		{

		}
	}
}
