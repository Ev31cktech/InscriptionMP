﻿using Newtonsoft.Json.Linq;
using System;

namespace Inscryption_Server.DataTypes
{
	public class Runnable
	{
		public Runner Executer { get; }
		public Func<JObject, bool> Action { get; private set; }
		public Runnable(Func<JObject, bool> _action, Runner executer = Runner.Both)
		{
			Action = _action;
			Executer = executer;
		}
		public enum Runner
		{
			Server,
			Both
		}
	}
}
