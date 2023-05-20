using Inscription_Server;
using Inscription_Server.Events.ValueChanged;
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
		List<CardSlot> opponentSlots = new List<CardSlot>();
		List<CardSlot> playerSlots = new List<CardSlot>();
		ObservableList<Card> handCards;
		Stack<CardData> cards;

		public BoardView(Inscription_Server.Scenes.BoardScene scene) : base(scene)
		{
			InitializeComponent();
			uint columnCount = 4;//TODO settings.Board.ColumnCount;
			if (thisScene.Team == Team.one)
			{
				thisScene.Team1Slots.ValueChanged += plyerSlots_Update;
				thisScene.Team2Slots.ValueChanged += opponentSlots_Update;
			}
			else
			{
				thisScene.Team2Slots.ValueChanged += plyerSlots_Update;
				thisScene.Team1Slots.ValueChanged += opponentSlots_Update;
			}

			for (int i = 0; i < columnCount; i++)
			{
				CardSlot pSlot = new CardSlot(this);
				CardSlot oSlot = new CardSlot(this);
				playerSlots.Add(oSlot);
				opponentSlots.Add(pSlot);
				OpponentSTP.Children.Add(oSlot);
				PlayerSTP.Children.Add(pSlot);
			}
		}
		internal void plyerSlots_Update(object sender, ValueChangedEventArgs e)
		{

		}
		internal void opponentSlots_Update(object sender, ValueChangedEventArgs e)
		{

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
