using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Inscription_Server.Scenes
{
	public class SetupScene : Scene
	{
		List<string> team1 = new List<string>();
		List<string> team2 = new List<string>();
		public SetupScene() : base()
		{
		}
		public void AddPlayer(string player)
		{
			team1.Add(player);
		}
	}
}
