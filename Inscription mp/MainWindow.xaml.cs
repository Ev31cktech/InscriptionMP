using Server;
using System;
using System.Windows;
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
		public Client client;
		public MainWindow()
		{
			InitializeComponent();
			mw = this;
			if (Properties.Settings.Default.AllSettings != "")
			{ Settings = JsonConvert.DeserializeObject<Settings>(Properties.Settings.Default.AllSettings); }
			else { Settings = new Settings(); }
			Closing += MainWindow_Closing;
		}
		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Properties.Settings.Default.AllSettings = JsonConvert.SerializeObject(Settings);
			Properties.Settings.Default.Save();
		}
	}
}
