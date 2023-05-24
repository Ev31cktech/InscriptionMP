using System;

namespace Inscription_Server.Events.ValueChanged
{
	public interface INotifyValueChanged
	{
		event ValueChangedEventHandler ValueChanged;
		void OnValueChanged(ValueChangedEventArgs e);

	}
	public delegate void ValueChangedEventHandler(Object sender, ValueChangedEventArgs e);
	public class ValueChangedEventArgs : EventArgs
	{
		public Action performed { get; set; }
		public object Value { get; private set; }
		public ValueChangedEventArgs(Action action, object item)
		{
			performed = action;
			Value = item;
		}
		public enum Action
		{
			Add,
			Remove,
			Move,
			Create,
			Update
		}
	}
}
