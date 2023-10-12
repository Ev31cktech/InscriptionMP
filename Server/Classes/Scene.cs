using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Inscryption_Server.Events.ValueChanged;
using Inscryption_Server.Events.INotifyEvent;
using Inscryption_Server.Serialization;
using Inscryption_Server.Exceptions.SceneExceptions;
using System.Linq;
using Inscryption_Server.Exceptions;
using Inscryption_Server.DataTypes;
using static Inscryption_Server.DataTypes.Runnable;

namespace Inscryption_Server
{
	public abstract class Scene : INotifyActionRun, IToJObject
	{
		private static JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings() { ContractResolver = new IgnoreFieldsContractResolver() });
		private Dictionary<string, Runnable> actions = new Dictionary<string, Runnable>();
		private static Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

		public event NotifyActionRunEventHandler ActionRunEvent;

		[JsonIgnore]
		public String SceneName { get { return GetType().FullName; } }

		public Scene()
		{
			actions.Add(GetActionName(Sync), new Runnable(Sync));
		}
		protected void InitializeScene(params Runnable[] funcs)
		{
			funcs.ToList().ForEach(i => this.actions.Add(GetActionName(i.Action), i));
		}

		internal bool Sync(JObject data)
		{
			JToken nameToken = null;
			JToken dataToken = null;
			if (!(data.TryGetValue("sceneName", out nameToken) && data.TryGetValue("sceneData", out dataToken)))
				throw new Exception("Sync data did not contain the right tokens");
			if (!Equals((nameToken as JValue).Value, SceneName))
				throw new Exception("Scenes did not match");
			Type thisObj = GetType();
			Sync(dataToken, thisObj, this);
			return true;
		}

		internal void Sync(JToken data, Type parentType, object var)
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

		private void Sync(JProperty parent, JArray jarr, object var)
		{
			PropertyInfo pi = var.GetType().GetRuntimeProperty(parent.Name);
			if (!pi.PropertyType.IsGenericType) //TODO does this include Array's?
			{ Sync(parent.Name, jarr.Children(), var); }
			else
			{   //Property is a list

				Type elementType = pi.PropertyType.GenericTypeArguments[0];                 //find the element Type of the list
				object element = null;
				IList genericArray = (IList)pi.GetValue(this);                              //copy the existing list
				genericArray.Clear();                                                       //clear the existing list
				foreach (JToken token in jarr.Children())
				{
					if ((token as JValue).Value == null)                                    //check if there is a value in the first place
					{ }
					else if (elementType.IsPrimitive || (elementType == typeof(string)))    //check if the element is a primative
					{ element = (token as JValue).Value; }
					//else if (elementType.GetInterfaces().Contains(typeof(IToJObject)))		//if implements IToObject, it has a constructor with 1 paramater of type JObject
					//{ 
					//	if(elementType.GetContrustor(new Type[] {typeof(JObject)}))
					//		{ throw new UnknownActionException($"{elementType.GetType()} implemented {nameof(IToJObject)} but did not implement a constructor with constructor signatur: ..ctor(JObject). this is a must but can't be enforced by C#"); }
					//	element = Activator.CreateInstance(elementType, token as JObject); //TODO test if this works
					//	Sync(token, elementType, element);
					//}
					else
					{
						element = Activator.CreateInstance(elementType);
						Sync(token, elementType, element);
					}
					genericArray.Add(element);
				}
				Sync(parent.Name, genericArray, var);
			}
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
				(value as INotifyValueChanged).OnValueChanged(new ValueChangedEventArgs(ValueChangedEventArgs.Action.Update, value));
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

		internal static Scene GetScene(JObject data)
		{
			Scene temp;
			if (!scenes.TryGetValue(data["sceneName"].Value<string>(), out temp))
			{ throw new UnknownSceneException(); }
			temp.Sync(data);
			return temp;
		}
		internal static string GetActionName(Func<JObject, bool> action)
		{
			return $"{action.Method.DeclaringType.FullName}.{action.Method.Name}";
		}
		internal void TryRunAction(Func<JObject, bool> func, JObject data, Client sender = null)
		{
			Runnable action;
			if (!actions.TryGetValue(GetActionName(func), out action))
				throw new ActionNotFoundExceptionException();
			ActionRunEventData e = new ActionRunEventData(action, data);
			App.Logger.Info($"Action {GetActionName(func)} run");
			OnActionRun(e);
			if (e.Executer == Runner.Server)
			{
				if (sender != null)
					return;
			}
			e.Invoke();
		}

		internal void TryRunAction(string func, JObject data, Client sender)
		{
			Runnable action;
			if (!actions.TryGetValue(func, out action))
				throw new ActionNotFoundExceptionException();
			ActionRunEventData e = new ActionRunEventData(action, data);
			App.Logger.Info($"Action {func.Split('.').Last()} recieved and run");
			if (!e.Invoke())
			{ sender.AddAction(Sync, ToJObject()); }
			else
			{ OnActionRun(e, sender); }
		}

		public void OnActionRun(ActionRunEventData e, Client sender = null)
		{
			ActionRunEvent?.Invoke(sender, e);
		}
	}
	public abstract class Scene<T> : Scene where T : Scene
	{
		public static T thisScene { get; private set; }
		public Scene()
		{ thisScene = this as T; }
	}
}