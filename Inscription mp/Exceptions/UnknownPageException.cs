using System;

namespace Inscription_mp.Exceptions
{

	[Serializable]
	public class UnknownViewException : Exception
	{
		public UnknownViewException() : base("Unknown View") { }
		public UnknownViewException(string message) : base(message) { }
		public UnknownViewException(string message, Exception inner) : base(message, inner) { }
		protected UnknownViewException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
