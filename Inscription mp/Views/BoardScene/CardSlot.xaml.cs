using System;
using System.Windows.Controls;

namespace Inscryption_mp.Views.BoardScene
{
	/// <summary>
	/// Interaction logic for CardSlot.xaml
	/// </summary>
	internal partial class CardSlot : UserControl
	{
		BoardView boardView;
		public bool HasCard { get { return cardSlot.Child != null; } }
		public Card Card
		{
			get { return cardSlot.Child as Card; }
			set
			{
				if (!HasCard)
				{ cardSlot.Child = value; }
			}
		}
		public CardSlot(BoardView bv)
		{
			MouseEnter += (s, e) => { boardView.SetActive(this); };
			boardView = bv;
			InitializeComponent();
		}
	}
}
