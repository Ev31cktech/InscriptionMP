using Newtonsoft.Json.Linq;

namespace Inscryption_Server.Serialization
{
	public interface IToJObject
	{
		//void FromObject(JObject data);	
		JObject ToJObject();
	}
}
