using Inscryption_mp.Views.BoardScene;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CardData = Inscryption_mp.DataTypes.CardData;

namespace Inscryption_mp
{
	/// <summary>
	/// Interaction logic for Card.xaml
	/// </summary>
	internal partial class Card : UserControl
	{
		private CardSlot slot;
		//EventHandler<CardEventArgs> OnSummon;
		private CardData Data { get; }
		public Card(CardData card)
		{
			Data = card;
			InitializeComponent();
			CardName.Content = card.Name;
			HealthPNL.Content = card.Health;
			PowerPNL.Content = card.Power;
			CardPortrait.Child = XamlReader.Parse(XamlWriter.Save(card.Portrait)) as UIElement;
		}
		public void Summon()
		{

		}
		public void Perish()
		{

		}
		public void Sacrifice()
		{
			Perish();
		}
		public void Attack(Card opponent)
		{
		}
		internal static bool TryGetCard(string cardId, out Inscryption_Server.DataTypes.CardData card) => Inscryption_Server.DataTypes.Card.TryGetCard(cardId, out card);
	}
}
