using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inscription_mp.Views
{
	/// <summary>
	/// Interaction logic for JoinGameView.xaml
	/// </summary>
	public partial class JoinGameView : View
	{
		public JoinGameView()
		{
			InitializeComponent();
		}

        public void JoinGame_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    MainWindow.ViewPrevious();
                    break;
            }
        }
        private void JoinGameButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                { App.JoinDedicatedServer(IPAddress.Loopback); }
                catch
                {  }
            });
        }
        private void BackToMainViewButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewPrevious();
        }
    }
}
