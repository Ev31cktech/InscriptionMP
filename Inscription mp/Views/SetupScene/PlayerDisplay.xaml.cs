using System;
using System.Windows;
using System.Windows.Controls;
using Inscription_Server.DataTypes;

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
			if (!(App.Client.IsHost && App.Client.UserID != Player.UserID)) //only the host has the power to kick and transfer host. and can't kick or , transfer host to himself.
			{
				HostMNI.Visibility = Visibility.Collapsed;
				KickMNI.Visibility = Visibility.Collapsed;
			}
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

		private void HostMNI_Click(object sender, RoutedEventArgs e)
		{
			PlayerDisplay pd = sender as PlayerDisplay;
			App.Client.TransferHost(pd.Player);
		}

		private void KickMNI_Click(object sender, RoutedEventArgs e)
		{
			PlayerDisplay pd = sender as PlayerDisplay;
			App.Client.KickPlayer(pd.Player);
		}

		private void SwitchTeamMNI_Click(object sender, RoutedEventArgs e)
		{
			SwitchTeam(this, e);
		}
	}
}
