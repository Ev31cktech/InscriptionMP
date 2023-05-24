using Inscription_Server.Events.ValueChanged;
using System.Collections.Generic;
using System.Linq;
using static Inscription_Server.Events.ValueChanged.ValueChangedEventArgs;

namespace Inscription_Server
{
	public class ObservableList<T> : List<T>, INotifyValueChanged
	{
		public bool Overidable { get; } = true;
		public bool Resizable { get; } = true;
		public ObservableList(bool overidable = true, bool resizable = true) : base()
		{
			Overidable = overidable;
			Resizable = resizable;
		}
		public ObservableList(IEnumerable<T> enumerable, bool overidable = true, bool resizable = true) :this(overidable, resizable)
		{ 
			base.AddRange(enumerable);
			OnValueChanged(new ValueChangedEventArgs(Action.Create, this));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="NotOveridableException">thrown when <see cref="Overidable"> is set to false and the index written to already has a value</exception>
		public new T this[int index]
		{
			get { return this[index]; }
			set
			{
				if (!Overidable && this[index] != null)
					throw new NotOveridableException();
				this[index] = value;
				OnValueChanged(new ValueChangedEventArgs(Action.Update, value));
			}
		}
		public new void Add(T item)
		{
			if (!Resizable)
				throw new NotResizableException();
			base.Add(item);
			OnValueChanged(new ValueChangedEventArgs(Action.Add, this));
		}
		public new bool Remove(T item)
		{
			bool rVal = base.Remove(item);
			OnValueChanged(new ValueChangedEventArgs(Action.Remove, item));
			return rVal;
		}
		public event ValueChangedEventHandler ValueChanged;

		/// <summary>
		///  Occurs when a property value changes.
		///     Raises the System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged
		///     event with the provided arguments.
		/// </summary>
		/// <param name="e">Arguments of the event being raised.</param>
		public void OnValueChanged(ValueChangedEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}

		[System.Serializable]
		public class NotOveridableException : System.Exception
		{
			public NotOveridableException() : base("List was set to not overidable but was attempted to") { }
			public NotOveridableException(string message) : base(message) { }
			public NotOveridableException(string message, System.Exception inner) : base(message, inner) { }
			protected NotOveridableException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}

		[System.Serializable]
		public class NotResizableException : System.Exception
		{
			public NotResizableException() : base("Resizable was set to false but tried to add") { }
			public NotResizableException(string message) : base(message) { }
			public NotResizableException(string message, System.Exception inner) : base(message, inner) { }
			protected NotResizableException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}
	}
}
