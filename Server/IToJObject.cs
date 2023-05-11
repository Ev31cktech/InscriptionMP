using Newtonsoft.Json.Linq;

namespace Inscription_Server.Serialization
{
	public interface IToJObject
	{
		JObject ToJObject();
	}
}
