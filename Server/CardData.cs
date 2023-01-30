using System;
using System.Xml.Linq;

namespace Server
{
	public struct CardData
	{
		public String Name {get; private set;}
		public String Species {get; private set;}
		//public CostData Cost {get; }
		public uint Health {get; set;}
		public uint Power {get; set;}

		public CardData(String name, uint health, uint power) : this(name,null,health,power){}
		public CardData(String name,String species,uint health, uint power)
		{
			Name = name;
			Species = species;
			Health = health;
			Power = power;
			//Cost = 
		}
	}
}
