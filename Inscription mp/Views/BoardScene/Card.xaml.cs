using Inscryption_mp.Views.BoardScene;
using Inscryption_Server;
using Inscryption_Server.DataTypes;
using System.Windows.Controls;

namespace Inscryption_mp
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
