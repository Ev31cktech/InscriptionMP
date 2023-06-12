namespace Inscryption_Server.Exceptions.SceneExceptions
{
	[System.Serializable]
	public class UnknownActionException : SceneException
	{
		public UnknownActionException() { }
		public UnknownActionException(string message) : base(message) { }
		public UnknownActionException(string message, System.Exception inner) : base(message, inner) { }
		protected UnknownActionException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
