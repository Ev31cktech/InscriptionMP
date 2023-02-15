using System;
using System.Collections.Generic;
using Inscription_Server.Exceptions.SceneExceptions;
using Inscription_Server.Scenes;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Inscription_Server
{
		[JsonObject(MemberSerialization.Fields)]
	public abstract class Scene
	{
		[JsonIgnore]
		private Client client;
		[JsonIgnore]
		private Dictionary<string,Action<JObject>> actions = new Dictionary<string,Action<JObject>>();
		[JsonIgnore]
		private static Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
		
		public Scene(params Action<JObject>[] funcs)
		{
			actions.Add(GetActionName(Sync), Sync);	
		}
		public void Sync(JObject data)
		{
			foreach (FieldInfo fi in this.GetType().GetFields())
			{ fi.SetValue(fi, data[fi.Name]);}
		}
		public void RunAction(Action<JObject> action, JObject data)
		{
			client.AddAction(action,data);
			action.Invoke(data);
		}
		public void RunAction(String func, JObject data)
		{
			Action<JObject> action;
			if (!actions.TryGetValue(func, out action))
			{ throw new UnknownActionException();}
			action.Invoke(data);
		}
		public virtual JObject ToJObject()
		{
			JObject obj = JObject.Parse($"{{\"sceneName\":\"{GetType().FullName}\"}}");
			obj.Add("data",JToken.FromObject(this));
			return obj;
		}
		public static void RegisterScene(Scene scene)
		{
			scenes.Add(scene.GetType().FullName, scene);
		}
		public static Scene GetScene(JObject data,Client client)
		{
			Scene temp;
			if(!scenes.TryGetValue(data["sceneName"].Value<string>(),out temp))
			{ throw new UnknownSceneException(); }
			temp = Activator.CreateInstance(temp.GetType()) as Scene;
			temp.client = client;
			return temp;
		}
		public static string GetActionName(Action<JObject> action)
		{
			return $"{action.Target.GetType().FullName}.{action.Method.Name}";
		}
	}
}