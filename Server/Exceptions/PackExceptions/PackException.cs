using System;

namespace Inscryption_Server.Exceptions.PackExceptions
{

	[Serializable]
	internal class PackException : Exception
	{
		public PackException() : base("Error in card pack") { }
		public PackException(string message) : base(message) { }
		public PackException(string message, Exception inner) : base(message, inner) { }
		protected PackException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
