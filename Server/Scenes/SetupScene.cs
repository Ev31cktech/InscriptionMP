using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inscription_Server.Scenes
{
	public class SetupScene : Scene
	{
		public String[] Team1 { get { return team1.ToArray(); } private set { team1 = value.ToList(); } }
		public String[] Team2 { get { return team2.ToArray(); } private set { team2 = value.ToList(); } }
		List<string> team1 = new List<string>();
		List<string> team2 = new List<string>();
		public SetupScene() : base()
		{}
		public void AddPlayer()//string player)
		{
			string player = $"testp{team1.Count + team2.Count}"; 
			if (team2.Count < team1.Count)
			{ team1.Add(player); }
			else
			{ team2.Add(player); }
		}
	}
}
