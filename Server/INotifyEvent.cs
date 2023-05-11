using Newtonsoft.Json.Linq;
using System;

namespace Inscription_Server.Events.INotifyEvent
{
	public interface INotifyActionRun
	{
		event NotifyActionRunEventHandler ActionRunEvent;
		void OnActionRun(Client sender, ActionRunEventData e);

	}
	public delegate void NotifyActionRunEventHandler(Client sender, ActionRunEventData e);
	public class ActionRunEventData
	{
		public Action<JObject> Action { get; }
		public JObject Data { get; }
		public ActionRunEventData(Action<JObject> action,JObject data)
		{
			Action = action;
			Data = data;
		}
	}
}
