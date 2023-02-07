using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Inscription_mp.View
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : Page
	{
		Button[] buttons;
		public MainView()
		{
			InitializeComponent();
			buttons = new Button[]{
				CreateGameBTN,
				JoinGameBTN,
				SettingsGameBTN,
				ExitGameBTN
			};
		}

		private void CreateGameButton_Click(object sender, RoutedEventArgs e)
		{
			App.StartLocalServer();
			ToggleButtons();
			//MainWindow.MainWindow_ShowView(new CreateGameView());
		}

		private void JoinGameButton_Click(object sender, RoutedEventArgs e)
		{
			App.JoinDedicatedServer(IPAddress.Loopback);
			ToggleButtons();
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.Mainwindow_ShowSettingsView();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.Exit();
		}
		private void ToggleButtons()
		{
			CreateGameBTN.IsEnabled = false;
			JoinGameBTN.IsEnabled = false;
		}
	}
}
