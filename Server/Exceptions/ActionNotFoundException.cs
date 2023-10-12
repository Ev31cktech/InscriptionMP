using System;

namespace Inscryption_Server.Exceptions
{
	[Serializable]
	internal class ActionNotFoundExceptionException : System.Exception
	{
		public ActionNotFoundExceptionException() { }
		public ActionNotFoundExceptionException(string message) : base(message) { }
		public ActionNotFoundExceptionException(string message, System.Exception inner) : base(message, inner) { }
		protected ActionNotFoundExceptionException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}