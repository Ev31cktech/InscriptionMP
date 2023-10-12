using System;
using System.Collections.Generic;
using System.Linq;

namespace Inscryption_Server.DataTypes
{
	public abstract class Card
	{
		private static Dictionary<string,CardData> cards = new Dictionary<string,CardData>();
		public uint Health { get; private set; }
		public uint Power { get; private set; }
		public string Name { get => CardData.Name; }
		public CardData CardData {get; }
		public Card(CardData cardData)
		{
			Health = cardData.Health;
			Power = cardData.Power;
			CardData = cardData;
		}
		public virtual void OnAttack()
		{}
		public virtual void OnDamage()
		{}
		public virtual void OnSummon()
		{}
		public virtual void OnDeath()
		{}
		public virtual void OnBeginTurn()
		{}
		public virtual void OnSacrifice()
		{}
		public virtual void OnDraw()
		{}
		public virtual void OnAction()
		{}
		public abstract bool CanPlay(CardData card);
		public abstract void OnPlay();
		internal static bool TryGetCard(string cardId, out CardData card){
			return cards.TryGetValue(cardId,out card);
		}
		internal static void RegisterCard(CardData cardData)
		{
			cards.Add(cardData.CardId, cardData);
		}
	}
}
