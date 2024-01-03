using System;

namespace Inscryption_mp.Exceptions
{

	[Serializable]
	public class UnknownCardException : ClientException
	{
		public UnknownCardException() { }
		public UnknownCardException(string message) : base(message) { }
		public UnknownCardException(string message, Exception inner) : base(message, inner) { }
		protected UnknownCardException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
