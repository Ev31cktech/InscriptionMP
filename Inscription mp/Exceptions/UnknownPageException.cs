using System;

namespace Inscryption_mp.Exceptions
{

	[Serializable]
	internal class UnknownViewException : UnknownCardException
	{
		public UnknownViewException() : base("Unknown View") { }
		public UnknownViewException(string message) : base(message) { }
		public UnknownViewException(string message, UnknownCardException inner) : base(message, inner) { }
		protected UnknownViewException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
