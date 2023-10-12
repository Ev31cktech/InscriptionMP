using System;

namespace Inscryption_Server.Exceptions.PackExceptions
{

	[Serializable]
	internal class CardException : PackException
	{
		public CardException() { }
		public CardException(string message) : base(message) { }
		public CardException(string message, Exception inner) : base(message, inner) { }
		protected CardException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
