using Inscryption_Server.DataTypes;
using Inscryption_Server.Serialization;
using Newtonsoft.Json.Linq;

namespace Inscryption_Server.DataTypes
{
	public struct Player : IToJObject
	{
		public uint UserID { get; private set; }
		public string Username { get; private set; }
		public Team Team { get; set; }
		public Player(uint userID, string username, Team team)
		{
			UserID = userID;
			Username = username;
			Team = team;
		}
		public Player(JObject data)
		{
			UserID = data.Value<uint>("UserID");
			Username = data.Value<string>("Username");
			Team = (Team)data.Value<int>("Team");
		}

		public JObject ToJObject()
		{
			return JObject.FromObject(this);
		}
	}
}
