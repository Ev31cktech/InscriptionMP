using System;
using System.Windows;
using System.Windows.Controls;

namespace Inscription_mp.Views.SetupScene
{
	/// <summary>
	/// Interaction logic for PlayerDisplay.xaml
	/// </summary>
	public partial class PlayerDisplay : UserControl
	{
		public enum Team
		{
			one,
			two
		}
		public PlayerDisplay(string Name, Team team)
		{
			InitializeComponent();
			ChangeTeam(team);
		}
		public void ChangeTeam(Team team)
		{
			SwitchTeamLeftBTN.Visibility = Visibility.Hidden;
			SwitchTeamRightBTN.Visibility = Visibility.Hidden;

			switch (team)
			{
				case Team.one:
					break;
				case Team.two:
					break;
			}
		}
		public void SwitchTeam(object sender, EventArgs e)
		{

		}
	}
}
