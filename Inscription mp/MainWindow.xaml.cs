using System;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inscription_mp.Scenes.MainScene;
using Newtonsoft.Json;


namespace Inscription_mp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static MainWindow mw;
		public static Settings Settings;
		private Scene scene;
		public static Scene Scene
		{
			get {return mw.scene;}
			set
			{
				mw.scene = value;
				mw.Content = value.CurrentView;
			}
		}
		public MainWindow()
		{
			InitializeComponent();
			mw = this;
			if (Properties.Settings.Default.AllSettings != "")
			{ Settings = JsonConvert.DeserializeObject<Settings>(Properties.Settings.Default.AllSettings); }
			else { Settings = new Settings(); }
			Scene = new MainScene();
			this.KeyDown += (s, e) => { scene.Scene_KeyDown(s, e); };
			Closing += MainWindow_Closing;
		}
		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Properties.Settings.Default.AllSettings = JsonConvert.SerializeObject(Settings);
			Properties.Settings.Default.Save();
		}
		public static void Exit()
		{
			mw.Close();
		}
	}
}
