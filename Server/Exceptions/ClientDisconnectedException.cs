using System;

namespace Inscryption_Server.Exceptions
{

	[Serializable]
	public class ClientDisconnectedException : Exception
	{
		public ClientDisconnectedException() : base("Client disconnected") { }
		public ClientDisconnectedException(string message) : base(message) { }
		public ClientDisconnectedException(string message, Exception inner) : base(message, inner) { }
		protected ClientDisconnectedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
