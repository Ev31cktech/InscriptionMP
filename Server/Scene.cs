using System;
using System.Collections.Generic;
using Inscription_Server.Exceptions.SceneExceptions;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Inscription_Server
{
	//[JsonObject(MemberSerialization.Fields)]
	public abstract class Scene
	{
		private Client client;
		private Dictionary<string,Action<Client,JObject>> actions = new Dictionary<string,Action<Client,JObject>>();
		private static Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

		public Scene(params Action<JObject, Client>[] funcs)
		{
			actions.Add(GetActionName(Sync), Sync);	
		}
		public void Sync(Client client, JObject data)
		{
			foreach (PropertyInfo fi in GetType().GetProperties())
			{ 
				var value = JsonConvert.DeserializeObject<String[]>(data[fi.Name].ToString());
				fi.SetValue(this, value);
			}
		}
		public void RunAction(Action<Client,JObject> action,Client client, JObject data)
		{
			client.AddAction(action,data);
			action.Invoke(client,data);
		}
		public void RunAction(String func,Client client, JObject data)
		{
			Action<Client, JObject> action;
			if (!actions.TryGetValue(func, out action))
			{ throw new UnknownActionException();}
			action.Invoke(client,data);
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
		public static Scene GetScene(Client client,JObject data)
		{
			Scene temp;
			if(!scenes.TryGetValue(data["sceneName"].Value<string>(),out temp))
			{ throw new UnknownSceneException(); }
			temp = Activator.CreateInstance(temp.GetType()) as Scene;
			temp.client = client;
			temp.Sync(client, data["data"].Value<JObject>());
			return temp;
		}
		public static string GetActionName(Action<Client,JObject> action)
		{
			return $"{action.Target.GetType().FullName}.{action.Method.Name}";
		}
	}
}