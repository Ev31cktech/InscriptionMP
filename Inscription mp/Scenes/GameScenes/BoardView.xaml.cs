using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Inscription_mp.Scenes.GameScenes
{
	/// <summary>
	/// Interaction logic for BoardView.xaml
	/// </summary>
	public partial class BoardView : Page
	{
		CardSlots[][] cardSlots;
		public BoardView(GameRules settings)
		{
			InitializeComponent();
			for (int y = 0; y < 2; y++)
			{ 
				BoardGrid.RowDefinitions.Add(new RowDefinition());
				for (int x = 0; x < 4; x++)
				{ 
					if(y == 0)
					{  BoardGrid.ColumnDefinitions.Add(new ColumnDefinition()); }
					CardSlots slot = new CardSlots();
					Grid.SetColumn(slot,x);
					Grid.SetRow(slot,y);
					BoardGrid.Children.Add(slot);
				}
			}
		}
	}
	public class CardSlots : Button
	{
		public CardSlots()
		{
			Click += onClick;
			Background = Brushes.Transparent;
			//TODO set blank card background
		}
		public void onClick(object sender, EventArgs eventArgs)
		{

		}
	}
}
