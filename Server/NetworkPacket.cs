﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inscription_Server
{
	public class NetworkPacket
	{
		public int PacketN {get;}
		public JArray data;
		public NetworkPacket(int packetN) 
		{
			PacketN = packetN;
			data = new JArray();
		}
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
