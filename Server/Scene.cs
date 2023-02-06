using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Inscription_Server
{
	public abstract class Scene
	{
		private Dictionary<string, Action<JObject>> actions;
		public Scene(params Action<JObject>[] sceneActions)
		{
			foreach (Action<JObject> action in sceneActions) {

			}
		}
	}
}