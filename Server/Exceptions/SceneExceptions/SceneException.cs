namespace Inscription_Server.Exceptions
{

	[System.Serializable]
	public class SceneException : System.Exception
	{
		public SceneException() : base("Exception in Scene class"){ }
		public SceneException(string message) : base(message) { }
		public SceneException(string message, System.Exception inner) : base(message, inner) { }
		protected SceneException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
