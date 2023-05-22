using Inscription_Server.Events.ValueChanged;

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
				OnValueChanged(new ValueChangedEventArgs(old, value));
			}
		}

		public event ValueChangedEventHandler ValueChanged;
		public void OnValueChanged(ValueChangedEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}
	}
}
