using Inscription_mp.Views.SetupScene;
using static Inscription_mp.Views.SetupScene.PlayerDisplay;

namespace Inscription_mp.Views
{
	/// <summary>
	/// Interaction logic for SetupView.xaml
	/// </summary>
	public partial class SetupView : View
	{
		private static SetupView setupView;
		private Inscription_Server.Scenes.SetupScene scene;
		public SetupView(Inscription_Server.Scenes.SetupScene scene)
		{
			this.scene = scene;
			InitializeComponent();
			Loaded += (s, e) => { StartGameBTN.IsEnabled = App.IsHost; };
		}
		public override void Update()
		{
			Team1.Children.Clear();
			Team2.Children.Clear();
			foreach (string name in scene.Team1)
			{
				PlayerDisplay p = new PlayerDisplay(name, Team.one);
				Team1.Children.Add(p);
			}
			foreach (string name in scene.Team2)
			{
				PlayerDisplay p = new PlayerDisplay(name, Team.two);
				Team2.Children.Add(p);
			}
		}
		public static void SwitchTeam(PlayerDisplay pd)
		{

		}

		private void StartGameBTN_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			App.Client.CurrentView.Update();
			//scene.RunAction(scene.Sync, App.Client, scene.ToJObject());
		}
	}
}
