using Newtonsoft.Json.Linq;

namespace Inscryption_Server.Serialization
{
	// ===vv=== not included in summary ===vv=== (concider commented out)
	/// <remark>Must include a constructor with a single parameter of type JObject</remark>
	/// This can not be inforced with the current version of C#...
	/// <summary>
	/// Interface to go to and from a JObject
	/// </summary>
	public interface IToJObject
	{	
		JObject ToJObject();
	}
}
