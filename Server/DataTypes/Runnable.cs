using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;

namespace Inscription_Server.DataTypes
{
	public class Runnable
	{
		public Runner Executer { get; }
		public Action<JObject> Action { get; private set; }
		public Runnable(Action<JObject> _action,Runner executer = Runner.Server)
		{
			Action = _action;
			Executer = executer;
		}
		public enum Runner
		{
			Server,
			Client,
			Both
		}
	}
}
