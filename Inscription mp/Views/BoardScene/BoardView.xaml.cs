using Inscription_Server;
using System.Collections.Generic;
using System.Windows.Input;

namespace Inscription_mp.Views.BoardScene
{
	/// <summary>
	/// Interaction logic for BoardView.xaml
	/// </summary>
	public partial class BoardView : View<Inscription_Server.Scenes.BoardScene>
	{
		CardSlot activeslot;
		ObservableList<CardSlot> team1slots = new ObservableList<CardSlot>();
		ObservableList<CardSlot> team2slots = new ObservableList<CardSlot>();
		List<CardData> handCards;
		Stack<CardData> cards;

		public BoardView(Inscription_Server.Scenes.BoardScene scene) : base(scene)
		{
			InitializeComponent(); 
			uint columnCount = 4;//TODO settings.Board.ColumnCount;
			for (int i = 0; i < columnCount; i++)
			{
				CardSlot pSlot = new CardSlot(this);
				CardSlot oSlot = new CardSlot(this);
				team1slots.Add(oSlot);
				team2slots.Add(pSlot);
			}
		}

		internal void SetActive(CardSlot cardSlot)
		{
			activeslot = cardSlot;
		}
		public void BoardGrid_MouseDown(object sender, MouseEventArgs e)
		{
			//thisScene.RunAction(thisScene.Summon,new CardData("test", 2, 2));
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				activeslot.Card = new Card(new CardData("test", 2, 2));
			}
		}
	}
}
