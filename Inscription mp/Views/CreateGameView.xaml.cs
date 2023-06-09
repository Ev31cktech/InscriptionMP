using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Inscription_mp.Views
{
	/// <summary>
	/// Interaction logic for CreateGameView.xaml
	/// </summary>
	public partial class CreateGameView : View
	{
		public CreateGameView()
		{
			InitializeComponent();
		}

		public void CreateGame_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					MainWindow.ViewPrevious();
					break;
			}
		}

		private void CreateGameButton_Click(object sender, RoutedEventArgs e)
		{
			Task.Run(() =>
			{
				try
				{ App.StartLocalServer(); }
				catch
				{ }
			});
		}

		private void BackToMainViewButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.ViewPrevious();
		}
		public override void Initialize()
		{
		}
	}
}
