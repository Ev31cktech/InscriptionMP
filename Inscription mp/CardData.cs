using System.Windows;

namespace Inscryption_mp
{
	public abstract class CardData : Inscryption_Server.DataTypes.CardData
	{
		public UIElement Portrait { get; set; }
		public CardData() : base() { }
	}
}