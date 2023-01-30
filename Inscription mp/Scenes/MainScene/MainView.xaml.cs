using System.Windows;
using System.Windows.Controls;

namespace Inscription_mp.Scenes.MainScene
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : Page
	{
		public MainView()
		{
			InitializeComponent();
		}

        private void CreateGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void JoinGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Exit();
        }
    }
}
