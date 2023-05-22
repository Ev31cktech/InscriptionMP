using System;

namespace Inscription_Server.Exceptions.SceneExceptions
{

	[Serializable]
	public class UnknownSceneException : SceneException
	{
		public UnknownSceneException() :base("unknown scene encountered") { }
		public UnknownSceneException(string message) : base(message) { }
		public UnknownSceneException(string message, Exception inner) : base(message, inner) { }
		protected UnknownSceneException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
