using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Inscryption_mp.Views;
using Inscryption_mp.Views.BoardScene;
using Inscryption_Server.Scenes;

namespace Inscryption_mp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	internal partial class MainWindow : Window
	{
		private static MainWindow mw;
		private static SettingsView settingsView;
		private static Stack<View> viewStack = new Stack<View>();
		public MainWindow()
		{
			mw = this;
			InitializeComponent();
			BoardScene boardScene = new BoardScene();
			boardScene.settings.Board.ColumnCount = 4;
			BoardView boardView = new BoardView(boardScene);
			boardView.Initialize();
			MainWindow_ShowView(boardView);
			Loaded += (s,e) => { 
				App.Initializer.Initialize();
			}; //TODO progress bar
			
			settingsView = new SettingsView();
			Left = App.Settings.window.Left; Top = App.Settings.window.Top;
			WindowState = App.Settings.window.State;
		}

		/// <summary>
		/// shows the settings window
		/// <remark value="has it's own function because settings is difficult" />
		/// </summary>
		internal static void MainWindow_ShowSettingsView()
		{
			viewStack.Push(settingsView);
			mw.ShowView();
		}
		internal static void MainWindow_ShowView(View view)
		{
			viewStack.Push(view);
			mw.Dispatcher.InvokeAsync(mw.ShowView);
		}
		private void ShowView()
		{
			Content = viewStack.Peek();
			UpdateLayout();
			(Content as View).Focus();
		}
		public static void ShowMainView()
		{
			viewStack = new Stack<View>(new List<View>() { new MainView()});
			mw.Dispatcher.InvokeAsync(mw.ShowView);
		}

		public static void Exit()
		{
			mw.Close();
		}

		internal static void ViewPrevious()
		{
			viewStack.Pop();
			if(viewStack.Count >= 0)
				ShowMainView();
			mw.Dispatcher.InvokeAsync(mw.ShowView);
		}
	}
}
