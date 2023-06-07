using Inscription_Server.DataTypes;
using Newtonsoft.Json.Linq;
using System;

namespace Inscription_Server.Events.INotifyEvent
{
	public interface INotifyActionRun
	{
		event NotifyActionRunEventHandler ActionRunEvent;
		void OnActionRun(Client sender,ActionRunEventData e);

	}
	public delegate void NotifyActionRunEventHandler(Client sender, ActionRunEventData e);
	public class ActionRunEventData
	{
		public Runnable Runnable { get; }
		public JObject Data { get; }
		public ActionRunEventData(Runnable runnable,JObject data)
		{
			Runnable = runnable;
			Data = data;
		}
	}
}
