using Inscription_Server.Events.ValueChanged;
using System.Collections.Generic;

namespace Inscription_Server
{
	public class ObservableList<T> : List<T>, INotifyValueChanged
	{
		public ObservableList() : base()
		{}
		public ObservableList(IEnumerable<T> enumerable) : base(enumerable)
		{ OnValueChanged(new ValueChangedEventArgs(_new: this)); }
		public ObservableList(List<T> list) : base(list)
		{ OnValueChanged(new ValueChangedEventArgs(_new : this)); }
		public new T this[int index] { 
			get { return this[index]; } 
			set {
				this[index] = value;
				OnValueChanged(new ValueChangedEventArgs()); 
			}
		}
		public new void Add(T item)
		{
			base.Add(item);
			OnValueChanged(new ValueChangedEventArgs(_new: this));
		}
		public new bool Remove(T item)
		{
			bool rVal = base.Remove(item);
			OnValueChanged(new ValueChangedEventArgs(item));
			return rVal;
		}
		public event ValueChangedEventHandler ValueChanged;

		//
		// Summary:
		//     Occurs when a property value changes.

		//
		// Summary:
		//     Raises the System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged
		//     event with the provided arguments.
		//
		// Parameters:
		//   e:
		//     Arguments of the event being raised.
		public void OnValueChanged(ValueChangedEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}
	}
}
