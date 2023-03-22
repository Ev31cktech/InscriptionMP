using System.Windows;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.ComponentModel;
using System;
using Inscription_mp.Views;

namespace Inscription_mp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Settings Settings { get{return settingsView.Settings; } }
		private static MainWindow mw;
		private static SettingsView settingsView;
		public MainWindow()
		{
			InitializeComponent();
			mw = this;
			Settings tempset = new Settings();
			if (Properties.Settings.Default.AllSettings != "")
			{ tempset = JsonConvert.DeserializeObject<Settings>(Properties.Settings.Default.AllSettings); }
			settingsView = new SettingsView(tempset);
			Left = Settings.window.Left; Top = Settings.window.Top;
			WindowState = Settings.window.State;
			Closing += MainWindow_Closing;
		}

		/// <summary>
		/// shows the settings window
		/// <remark value="has it's own function because settings is difficult" />
		/// </summary>
		public static void Mainwindow_ShowSettingsView()
		{
			MainWindow_ShowView(settingsView);
		}
		public static void MainWindow_ShowView(Page view)
		{
			mw.Dispatcher.Invoke(mw.ShowView, view);
		}
		private void ShowView(Page view)
		{
			Content = view;
		}
		private void MainWindow_Closing(object sender, CancelEventArgs e)
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
