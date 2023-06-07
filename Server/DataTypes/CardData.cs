using Inscription_Server.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Inscription_Server.DataTypes
{
	public struct CardData : IToJObject
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
		public JObject ToJObject()
		{
			return JObject.FromObject(this);
		}

		public void FromObject(JObject data)
		{
		}
	}
}
