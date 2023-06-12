using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inscryption_Server.DataTypes
{
	public class NetworkPacket
	{
		public uint PacketN {get;}
		public JArray actions;
		public JArray data;
		public NetworkPacket(uint packetN) 
		{
			PacketN = packetN;
			data = new JArray();
			actions	= new JArray();
		}
		public NetworkPacket(JObject data) 
		{
			this.actions = data.ContainsKey("actions") ? data.Value<JArray>("actions") : new JArray();
			this.data =  data.ContainsKey("data") ? data.Value<JArray>("data") : new JArray();
			PacketN = data.Value<uint>("Packetn");
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
