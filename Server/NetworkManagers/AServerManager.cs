using Inscription_Server.Scenes;
using Newtonsoft.Json.Linq;

namespace Inscription_Server.NetworkManagers
{
	public abstract class AServerManager
	{
		public bool SetupState {get; set;}= true;
		public Scene CommonScene { get; private set; }
		public AServerManager()
		{
			CommonScene = new SetupScene();
		}
		public abstract void Loop();
		public abstract void Shutdown();
	}
}