using Inscryption_Server.DataTypes;

namespace Act1_CardPack.Cards
{
	public class AnimalCardData : CardData
	{
		public string ElderlyID { get; set; }
		public string BabyID { get; set; }
		public AnimalCardData()
		{}

		public override bool CanPlay(CardData card)
		{
			return false;
		}

		public override void PlayCard()
		{}
	}
}
