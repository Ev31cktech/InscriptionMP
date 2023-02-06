using Newtonsoft.Json.Linq;

namespace Inscription_Server.NetworkManagers
{
	public abstract class AServerManager
	{
		JObject data;
		public void AddData(JObject data)
		{
			data.Add(data);	
		}
		public abstract void Loop();
		public abstract void Shutdown();
	}
}