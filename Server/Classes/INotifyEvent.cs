using Inscription_Server.DataTypes;
using Newtonsoft.Json.Linq;
using System;

namespace Inscription_Server.Events.INotifyEvent
{
	public interface INotifyActionRun
	{
		event NotifyActionRunEventHandler ActionRunEvent;
		void OnActionRun(ActionRunEventData e, Client sender);

	}
	public delegate void NotifyActionRunEventHandler(Client sender, ActionRunEventData e);
	public class ActionRunEventData
	{
		public Runnable Runnable { get; }
		public JObject Data { get; }
		public Runnable.Runner Executer { get => Runnable.Executer;}

		public ActionRunEventData(Runnable runnable,JObject data)
		{
			Runnable = runnable;
			Data = data;
		}

		public bool Invoke()
		{ return Runnable.Action.Invoke(Data); }
		public override string ToString()
		{
			return $"{Runnable.Action.Method.Name}( {Data} )";
		}
	}
}
