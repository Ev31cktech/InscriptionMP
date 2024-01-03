using Inscryption_Server.DataTypes;
using Inscryption_Server.Events.ValueChanged;
using System.Collections.Generic;
using System.Windows.Input;

namespace Inscryption_mp.Views.BoardScene
{
	/// <summary>
	/// Interaction logic for BoardView.xaml
	/// </summary>
	internal partial class BoardView : View<Inscryption_Server.Scenes.BoardScene>
	{
		CardSlot activeslot;
		CardSlot[] opponentSlots;
		CardSlot[] playerSlots;
		Stack<CardData> cards;

		public BoardView(Inscryption_Server.Scenes.BoardScene scene) : base(scene)
		{
			InitializeComponent();
		}
		public override void Initialize()
		{
			uint columnCount = thisScene.settings.Board.ColumnCount;
			playerSlots = new CardSlot[columnCount];
			opponentSlots = new CardSlot[columnCount];
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
				playerSlots[i] = oSlot;
				opponentSlots[i] = pSlot;
				OpponentSTP.Children.Add(oSlot);
				PlayerSTP.Children.Add(pSlot);
			}
		}
		public void plyerSlots_Update(object sender, ValueChangedEventArgs e)
		{

		}
		public void opponentSlots_Update(object sender, ValueChangedEventArgs e)
		{

		}

		public void SetActive(CardSlot cardSlot)
		{
			activeslot = cardSlot;
		}
		public void BoardGrid_MouseDown(object sender, MouseEventArgs e)
		{
			//thisScene.RunAction(thisScene.Summon,new CardData("test", 2, 2));
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				CardData data;
				if(Card.TryGetCard("Act1 Pack.Hodag", out data))
				{ throw new Exceptions.UnknownCardException(); }
				activeslot.Card = new Card(data as Inscryption_mp.DataTypes.CardData);
			}
		}
	}
}
