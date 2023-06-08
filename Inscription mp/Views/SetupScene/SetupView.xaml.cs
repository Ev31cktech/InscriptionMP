using Inscription_mp.Views.SetupScene;
using System.Windows;

namespace Inscription_mp.Views
{
	/// <summary>
	/// Interaction logic for SetupView.xaml
	/// </summary>
	public partial class SetupView : View<Inscription_Server.Scenes.SetupScene>
	{
		public SetupView(Inscription_Server.Scenes.SetupScene scene) : base(scene)
		{
			InitializeComponent();
			Loaded += (s, e) => { StartGameBTN.IsEnabled = App.Client.IsHost; };
			scene.Team1.ValueChanged += (s,e) => { Dispatcher.Invoke(Team1_Update);};
			scene.Team2.ValueChanged += (s,e) => { Dispatcher.Invoke(Team2_Update);};
		}
		public void Team1_Update()
		{
			Team1STP.Children.Clear();
			foreach (var player in thisScene.Team1)
				Team1STP.Children.Add(new PlayerDisplay(player, this));
		}
		public void Team2_Update()
		{
			Team2STP.Children.Clear();
			foreach (var player in thisScene.Team2)
				Team2STP.Children.Add(new PlayerDisplay(player, this));
		}

		public void SwitchTeam(PlayerDisplay pd)
		{
			thisScene.TryRunAction(thisScene.SwitchTeam, pd.Player.ToJObject(), App.Client);
		}
		private void StartGameBTN_Click(object sender, RoutedEventArgs e)
		{
			thisScene.TryRunAction(thisScene.StartGame, thisScene.ToJObject(), App.Client);
			App.SoundEngine.StopSound();
		}
	}
}