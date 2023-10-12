using Inscryption_Server.DataTypes;
using System.Data;
using System.Diagnostics.Contracts;
using System.Runtime.Remoting.Messaging;

namespace Act1_CardPack
{
	public class BoneCard : Card
	{
		public static uint BoneCount { get; } = 0;
		public BoneCard(CardData cardData) : base(cardData)
		{
		}

		public override bool CanPlay(CardData boneCard)
		{
			return boneCard.Cost.Amount <= BoneCount;
		}

		public override void OnPlay()
		{
		}
	}
}