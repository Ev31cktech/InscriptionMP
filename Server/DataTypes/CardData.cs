using Inscryption_Server.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Inscryption_Server.DataTypes
{
	public abstract class CardData : IToJObject
	{
		public string CardId { get; private set; }
		public String Name { get; private set; }
		public String Species { get; private set; }
		public uint Health { get; private set; }
		public uint Power { get; private set; }
		public CostData Cost { get; private set; }
		public Sigil[] Sigils { get; private set; }
		public CardData()
		{ }

		internal void Initialize(string cardID, String name, uint health, uint power, CostData cost, Sigil[] sigils = null, String species = "")
		{
			CardId = cardID;
			Name = name;
			Species = species;
			Health = health;
			Power = power;
			Cost = cost;
			Sigils = sigils;
		}
		public JObject ToJObject()
		{
			return JObject.FromObject(this);
		}
		public abstract bool CanPlay(CardData card);
		public abstract void PlayCard();
	}
}
