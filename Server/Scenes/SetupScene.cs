using Newtonsoft.Json.Linq;
using System.Net.Sockets;

namespace Inscription_Server.Scenes
{
	internal class SetupScene : Scene
	{
		public SetupScene() : base(
			new System.Action<JObject>(Sync))
		{ }
		public static void Sync(JObject data)
		{

		}
	}
}
