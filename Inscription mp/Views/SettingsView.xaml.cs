using System.Windows.Controls;
using System.Windows.Input;

namespace Inscription_mp.Views
{
	/// <summary>
	/// Interaction logic for SettingsView.xaml
	/// </summary>
	public partial class SettingsView : View
	{
		public Settings Settings { get { return App.Settings; } }
		public SettingsView() 
		{
			InitializeComponent();
		}

		public override void Initialize()
		{
		}
		
		public void Settings_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					MainWindow.ViewPrevious();
					break;
			}
		}
	}
}