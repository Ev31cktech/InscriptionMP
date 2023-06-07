using Inscription_Server.Events.ValueChanged;
using static Inscription_Server.Events.ValueChanged.ValueChangedEventArgs;

namespace Inscription_Server
{
	public class ObservableProperty<T> : INotifyValueChanged
	{
		private T value;
		public T Value
		{
			get => value;
			set
			{
				T old = this.value;
				this.value = value;
				OnValueChanged(new ValueChangedEventArgs(Action.Update, value));
			}
		}

		public event ValueChangedEventHandler ValueChanged;
		public void OnValueChanged(ValueChangedEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}
	}
}
