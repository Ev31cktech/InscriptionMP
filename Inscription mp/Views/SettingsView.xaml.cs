using System.Windows.Controls;

namespace Inscription_mp.Views
{
	/// <summary>
	/// Interaction logic for SettingsView.xaml
	/// </summary>
	public partial class SettingsView : Page
	{
		public Settings Settings { get; private set; }
		public SettingsView(Settings settings)
		{
			Settings = settings;
			InitializeComponent();
		}
	}
}