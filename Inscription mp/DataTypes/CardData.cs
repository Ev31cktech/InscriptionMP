using System.Windows;

namespace Inscryption_mp.DataTypes
{
	public abstract class CardData : Inscryption_Server.DataTypes.CardData
	{
		public UIElement Portrait { get; set; }
		public CardData() : base() { }
	}
}