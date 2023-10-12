using System;

namespace Inscryption_Server.Exceptions.SceneExceptions
{

	[Serializable]
	internal class UnknownSceneException : SceneException
	{
		public UnknownSceneException() :base("unknown scene encountered") { }
		public UnknownSceneException(string message) : base(message) { }
		public UnknownSceneException(string message, Exception inner) : base(message, inner) { }
		protected UnknownSceneException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
