using System.Windows;
using Inscription_mp.Scenes;
using Newtonsoft.Json;


namespace Inscription_mp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static Settings Settings;
		private Scene scene = new GameScene();
		public MainWindow()
		{
			if (Properties.Settings.Default.AllSettings != "")
				Settings = JsonConvert.DeserializeObject<Settings>(Properties.Settings.Default.AllSettings);
			InitializeComponent();
			Canvas.KeyDown += (s, e) => scene.Scene_KeyDown(s, e);
			Closing += MainWindow_Closing;
		}

		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Properties.Settings.Default.AllSettings = JsonConvert.SerializeObject(Settings);
			Properties.Settings.Default.Save();
		}
	}
}
