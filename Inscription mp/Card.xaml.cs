using System;
using System.Windows.Controls;
using Server;

namespace Inscription_mp
{
	/// <summary>
	/// Interaction logic for Card.xaml
	/// </summary>
	public partial class Card : UserControl
	{
		private CardData data;
		EventHandler OnAttack;
		public Card(CardData card)
		{
			InitializeComponent();
		}
	}
}
