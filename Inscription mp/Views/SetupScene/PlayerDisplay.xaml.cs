using Inscription_Server;
using System;
using System.Windows;
using System.Windows.Controls;
using static Inscription_Server.Scenes.SetupScene;

namespace Inscription_mp.Views.SetupScene
{
	/// <summary>
	/// Interaction logic for PlayerDisplay.xaml
	/// </summary>
	public partial class PlayerDisplay : UserControl
	{
		private SetupView SetupView { get; }
		public Player Player { get; }

		public PlayerDisplay(Player player, SetupView parent)
		{
			Player = player;
			DataContext = this;
			SetupView = parent;
			InitializeComponent();
			ChangeTeam(Player.Team);
		}
		public void ChangeTeam(Team team)
		{
			SwitchTeamLeftBTN.Visibility = Visibility.Hidden;
			SwitchTeamRightBTN.Visibility = Visibility.Hidden;
			if (App.Client.IsHost || App.Client.UserID == Player.UserID)
			{
				switch (team)
				{
					case Team.one:
						SwitchTeamRightBTN.Visibility = Visibility.Visible;
						break;
					case Team.two:
						SwitchTeamLeftBTN.Visibility = Visibility.Visible;
						break;
				}
			}
		}
		public void SwitchTeam(object sender, EventArgs e)
		{
			SetupView.SwitchTeam(this);
		}
	}
}
