using Inscryption_mp.DataTypes;
using System.Collections.Generic;

namespace Act1_CardPack
{
	public class BoneCardData : CardData
	{
		//TODO can't be static because is per player relivant
		public static uint BoneCount { get; private set; } = 0;
		public BoneCardData(): base()
		{}

		public override bool CanPlay(Inscryption_Server.DataTypes.CardData boneCard) => boneCard.Cost.Amount <= BoneCount;

		public override void PlayCard(){}
	}
}