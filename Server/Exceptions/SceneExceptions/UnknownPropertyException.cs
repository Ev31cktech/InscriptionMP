using System;

namespace Inscription_Server.Exceptions.SceneExceptions
{

	[Serializable]
	public class UnknownPropertyException : Exception
	{
		public UnknownPropertyException() : base("Unknown Property") { }
		public UnknownPropertyException(string name, object var) : base($"could not find property {name} in {var.GetType()}.\n Property did not exist or is not public") { }
		public UnknownPropertyException(string message) : base(message) { }
		public UnknownPropertyException(string message, Exception inner) : base(message, inner) { }
		protected UnknownPropertyException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
