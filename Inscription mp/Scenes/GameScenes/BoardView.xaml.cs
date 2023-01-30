using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Inscription_mp.Scenes.GameScenes
{
	/// <summary>
	/// Interaction logic for BoardView.xaml
	/// </summary>
	public partial class BoardView : Page
	{
		CardSlot[] playerSlots;
		CardSlot[] opponentSlots;
		CardSlot activeslot;

		public BoardView(GameRules settings)
		{
			InitializeComponent();
			uint columnCount = 4;//TODO settings.Board.ColumnCount;
			playerSlots = new CardSlot[columnCount];
			opponentSlots = new CardSlot[columnCount];
			for (int i = 0; i < columnCount; i++)
			{
				BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
				CardSlot pSlot = new CardSlot(this);
				CardSlot oSlot = new CardSlot(this);
				Grid.SetColumn(pSlot, i);
				Grid.SetRow(pSlot, 1);
				Grid.SetColumn(oSlot, i);
				Grid.SetRow(oSlot, 0);
				playerSlots[i] = pSlot;
				opponentSlots[i] = oSlot;
				BoardGrid.Children.Add(oSlot);
				BoardGrid.Children.Add(pSlot);
			}
		}

		internal void Attack()
		{
			foreach (CardSlot slot in playerSlots)
			{
				//slot.Card.Attack();
			}
		}
		internal void Summon(Card card, uint slot)
		{

		}

		internal void SetActive(CardSlot cardSlot)
		{
			activeslot = cardSlot;
		}
		public void BoardGrid_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.LeftButton == MouseButtonState.Pressed && playerSlots.Contains(activeslot)){
				activeslot.Card = new Card(new Server.CardData("test",2,2));
			}
		}
	}
}
