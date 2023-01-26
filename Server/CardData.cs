using System;

namespace Server
{
	public struct CardData
	{
		public String Name {get; set;}
		public String Species {get; private set;}
		public uint Health {get; set;}
		public uint Power {get; set;}

	}
}
