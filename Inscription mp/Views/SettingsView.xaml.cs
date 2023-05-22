using System.Windows.Controls;

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
	}
}