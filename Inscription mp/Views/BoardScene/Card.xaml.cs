using Inscryption_mp.Views.BoardScene;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Inscryption_mp
{
	/// <summary>
	/// Interaction logic for Card.xaml
	/// </summary>
	internal partial class Card : UserControl
	{
		private CardSlot slot;
		//EventHandler<CardEventArgs> OnSummon;
		private CardData Data { get;}
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
	}
}
