using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using static Inscription_mp.SoundEngine;

namespace Inscription_mp.Views
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : View
	{
		public MainView()
		{
			InitializeComponent();
        }
		private void ButtonHover(object sender, RoutedEventArgs e)
		{
			App.SoundEngine.PlaySound(Sfx.Click);
		}

		private void CreateGameButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.MainWindow_ShowView(new CreateGameView());
		}

		private void JoinGameButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.MainWindow_ShowView(new JoinGameView());
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
			CreateGameBTN.IsEnabled = !CreateGameBTN.IsEnabled;
			JoinGameBTN.IsEnabled = !JoinGameBTN.IsEnabled;
		}
	}
}
