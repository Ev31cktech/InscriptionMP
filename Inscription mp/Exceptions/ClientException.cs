using System;

namespace Inscryption_mp.Exceptions
{
	[Serializable]
	public class ClientException : Exception
	{
		public ClientException() { }
		public ClientException(string message) : base(message) { }
		public ClientException(string message, Exception inner) : base(message, inner) { }
		protected ClientException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
