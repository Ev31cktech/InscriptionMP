namespace Inscription_mp.Exceptions
{

	[System.Serializable]
	public class SceneExceptions : System.Exception
	{
		public SceneExceptions() : base("Scene Exception"){ }
		public SceneExceptions(string message) : base(message) { }
		public SceneExceptions(string message, System.Exception inner) : base(message, inner) { }
		protected SceneExceptions(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
