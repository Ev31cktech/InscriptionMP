using Inscription_Server;
using Inscription_Server.Scenes;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;

namespace Inscription_mp.Views
{
	/// <summary>
	/// Interaction logic for GameRuleView.xaml
	/// </summary>
	public partial class SetupView : Page
	{
		private SetupScene scene;
		public SetupView(SetupScene scene)
		{
			this.scene = scene;
			InitializeComponent();
			Loaded += (s, e) => { StartGameBTN.IsEnabled = App.IsHost; };
		}


		private void StartGameBTN_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			scene.RunAction(scene.Sync,scene.ToJObject());
		}
	}
}
