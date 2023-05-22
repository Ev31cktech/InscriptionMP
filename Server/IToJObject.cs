using Newtonsoft.Json.Linq;

namespace Inscription_Server.Serialization
{
	public interface IToJObject
	{
		//void FromObject(JObject data);	
		JObject ToJObject();
	}
}
