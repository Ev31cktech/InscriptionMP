using Newtonsoft.Json.Linq;

namespace Server
{
	public abstract class Scene
	{
		public Scene()
		{}
		public abstract void Loop(JObject data);
	}
}