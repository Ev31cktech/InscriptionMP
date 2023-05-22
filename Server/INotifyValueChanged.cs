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
		public object Old { get; set; }
		public object New { get; set; }
		public ValueChangedEventArgs(object _old = null, object _new = null)
		{
			Old = _old;
			New = _new;
		}
	}
}
