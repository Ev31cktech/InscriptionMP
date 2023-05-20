using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Inscription_Server.Events.ValueChanged;
using Inscription_Server.Events.INotifyEvent;
using Inscription_Server.Serialization;
using Inscription_Server.Exceptions.SceneExceptions;
using System.Linq;
using Inscription_Server.Exceptions;

namespace Inscription_Server
{
	public abstract class Scene : INotifyActionRun, IToJObject
	{
		private static JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings() { ContractResolver = new IgnoreFieldsContractResolver() });
		private Dictionary<string, Action<JObject>> actions = new Dictionary<string, Action<JObject>>();
		private static Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

		public event NotifyActionRunEventHandler ActionRunEvent;

		[JsonIgnore]
		public object SceneName { get { return GetType().FullName; } }

		public Scene()
		{
			actions.Add(GetActionName(Sync), Sync);
		}
		protected void InitializeScene(params Action<JObject>[] funcs)
		{
			funcs.ToList().ForEach(i => this.actions.Add(GetActionName(i), i));
		}
		public void Sync(JObject data)
		{
			JToken nameToken = null;
			JToken dataToken = null;
			if (!(data.TryGetValue("sceneName", out nameToken) && data.TryGetValue("sceneData", out dataToken)))
				throw new Exception("Sync data did not contain the right tokens");
			if (!Equals((nameToken as JValue).Value, SceneName))
				throw new Exception("Scenes did not match");
			Type thisObj = GetType();
			Sync(dataToken, thisObj, this);
		}
		protected void Sync(JToken data, Type parentType, object var)
		{
			foreach (var token in data.Children())
			{
				JProperty parent = token.Parent as JProperty;
				switch (token.Type)
				{
					//==============================JValueTypes====================================
					case JTokenType.Integer:
					case JTokenType.Float:
					case JTokenType.String:
					case JTokenType.Boolean:
						Sync(parent.Name, (token as JValue).Value, var);
						break;
					case JTokenType.Array:
						Sync(parent, token as JArray, var);
						break;
					//===============================JTokenTypes===================================
					case JTokenType.Property:
						if (token.First.Type == JTokenType.Null)
							return;
						if (token.First.Type == JTokenType.Object)
						{
							Type pt = var.GetType().GetRuntimeProperty((token as JProperty).Name).PropertyType;
							var val = Activator.CreateInstance(pt);
							Sync(token, pt, val);
						}
						else
						{ Sync(token, parentType, var); }
						break;
					case JTokenType.Object:
						Sync(token, parentType, var);
						break;
					case JTokenType.Null:
						break;
					/*
					case JTokenType.None:
						break;
					case JTokenType.Constructor:
						break;
					case JTokenType.Comment:
						break;
					case JTokenType.Undefined:
						break;
					case JTokenType.Date:
						break;
					case JTokenType.Raw:
						break;
					case JTokenType.Bytes:
						break;
					case JTokenType.Guid:
						break;
					case JTokenType.Uri:
						break;
					case JTokenType.TimeSpan:
						break;
					*/
					default:
						throw new NotImplementedException($"Type '{token.Type}' is not implemented");
				}
			}
		}
		private void Sync(JProperty parent, JArray token, object var)
		{
			PropertyInfo pi = var.GetType().GetRuntimeProperty(parent.Name);
			if (pi.PropertyType.IsGenericType)
			{
				Type elementType = pi.PropertyType.GenericTypeArguments[0];
				object element = null;
				if (!elementType.IsPrimitive && !(elementType == typeof(string)))
					element = Activator.CreateInstance(elementType);
				IList genericArray = (IList)pi.GetValue(this);
				genericArray.Clear();
				foreach (JToken i in token.Children())
				{
					if (element != null)
						Sync(i, elementType, element);
					else
						element = (i as JValue).Value;
					genericArray.Add(element);
				}
				Sync(parent.Name, genericArray, var);
			}
			else
				Sync(parent.Name, token.Children(), var);
		}
		private void Sync(string name, object value, object var)
		{
			PropertyInfo pi = var.GetType().GetRuntimeProperty(name);
			if (pi == null)
				throw new UnknownPropertyException(name, var);
			if (pi.PropertyType.IsEnum)
				value = Enum.ToObject(pi.PropertyType, value);
			else
				value = Convert.ChangeType(value, pi.PropertyType);
			pi.SetValue(var, value);
			if (value is INotifyValueChanged)
				(value as INotifyValueChanged).OnValueChanged(new ValueChangedEventArgs(_new: value));
		}
		public virtual JObject ToJObject()
		{
			JObject obj = JObject.Parse($"{{\"sceneName\":\"{SceneName}\"}}");
			obj.Add("sceneData", JObject.FromObject(this, serializer));
			return obj;
		}
		public static void RegisterScene(Scene scene)
		{
			scenes.Add(scene.GetType().FullName, scene);
		}
		public static Scene GetScene(JObject data)
		{
			Scene temp;
			if (!scenes.TryGetValue(data["sceneName"].Value<string>(), out temp))
			{ throw new UnknownSceneException(); }
			temp.Sync(data);
			return temp;
		}
		public static string GetActionName(Action<JObject> action)
		{
			return $"{action.Target.GetType().FullName}.{action.Method.Name}";
		}
		public void RunAction(Client sender, Action<JObject> action, JObject data)
		{
			OnActionRun(sender, new ActionRunEventData(action, data));
		}
		public void TryRunAction(Client sender, string func, JObject data)
		{
			Action<JObject> action;
			if (!actions.TryGetValue(func, out action))
				throw new ActionNotFoundExceptionException();
			OnActionRun(sender, new ActionRunEventData(action, data));
		}

		public void OnActionRun(Client sender, ActionRunEventData e)
		{
			ActionRunEvent?.Invoke(sender, e);
		}
	}
}