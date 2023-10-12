using System;

namespace Inscryption_Server.Exceptions.PackExceptions
{

	[Serializable]
	internal class CostException : CardException
	{
		public CostException() { }
		public CostException(string message) : base(message) { }
		public CostException(string message, Exception inner) : base(message, inner) { }
		protected CostException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
