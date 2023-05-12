using Newtonsoft.Json.Linq;

namespace Inscription_Server.Scenes
{
	public class BoardScene : Scene
	{
		public JObject Data { get; }
		public BoardScene(JObject data) : base()
		{
			Data = data;
		}

	}
}
