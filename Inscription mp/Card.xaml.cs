using Inscription_mp.Views.BoardScene;
using Inscription_Server;
using System.Windows.Controls;

namespace Inscription_mp
{
	/// <summary>
	/// Interaction logic for Card.xaml
	/// </summary>
	public partial class Card : UserControl
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
