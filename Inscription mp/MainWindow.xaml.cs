using System.Windows;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.ComponentModel;
using Inscription_mp.Views;

namespace Inscription_mp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static MainWindow mw;
		private static SettingsView settingsView;
		public MainWindow()
		{
			InitializeComponent();
			mw = this;
			
			settingsView = new SettingsView();
			Left = App.Settings.window.Left; Top = App.Settings.window.Top;
			WindowState = App.Settings.window.State;
		}

		/// <summary>
		/// shows the settings window
		/// <remark value="has it's own function because settings is difficult" />
		/// </summary>
		public static void Mainwindow_ShowSettingsView()
		{
			mw.ShowView(settingsView);
		}
		public static void MainWindow_ShowView(View view)
		{
			mw.Dispatcher.InvokeAsync(() => { mw.ShowView(view); });
		}
		private void ShowView(UserControl view)
		{
			Content = view;
		}
		public static void ShowMainView()
		{
			mw.Dispatcher.InvokeAsync(() =>
			{
				MainWindow_ShowView(new MainView());
			});
		}

		public static void Exit()
		{
			mw.Close();
		}
	}
}
