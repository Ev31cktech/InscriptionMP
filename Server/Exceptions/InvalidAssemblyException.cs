using System;

namespace Inscryption_Server.Exceptions
{

	[Serializable]
	internal class InvalidAssemblyException : Exception
	{
		public InvalidAssemblyException() { }
		public InvalidAssemblyException(string message) : base(message) { }
		public InvalidAssemblyException(string message, Exception inner) : base(message, inner) { }
		protected InvalidAssemblyException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
