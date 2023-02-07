using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Inscription_Server
{
	public abstract class Scene
	{
		private Dictionary<string, Action<JObject>> actions = new Dictionary<string, Action<JObject>>();
		public Scene(params Action<JObject>[] sceneActions)
		{
			foreach (Action<JObject> action in sceneActions) {

			}
			actions.Add("scene.Change",new Action<JObject>(Change));
		}
		public void Change(JObject data)
		{

		}
	}
}