using Inscription_mp.Exceptions;
using Inscription_mp.Views;
using Inscription_Server;
using Inscription_Server.NetworkManagers;
using Inscription_Server.Scenes;
using log4net;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;
using PostSharp.Patterns.Diagnostics.Backends.Log4Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;

[assembly:Log]
namespace Inscription_mp
{
	public class App : Application
	{
		public static Server Server { get; private set; }
		public static Client Client { get; private set; }
		private static ILog Logger { get { return LogManager.GetLogger("CLIENT"); } }
		private static Timer timer;
		private static Dictionary<string, View> sceneViewList = new Dictionary<string, View>();
		private bool _contentLoaded;
		private static bool _consoleAttached;
		public void InitializeComponent()
		{
			if (_contentLoaded)
			{
				return;
			}
			_contentLoaded = true;
			this.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);
		}

#if DEBUG_CONSOLE
		[DllImport("kernel32.dll")]
		static extern bool AllocConsole();
#endif
		[DllImport("kernel32.dll")]
		static extern bool AttachConsole(int dwProcessId);
		[DllImport("kernel32.dll")]
		static extern bool FreeConsole();
		[DllImport("kernel32.dll")]
		static extern bool SetConsoleMode(IntPtr handle, int mode);
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		private const int ATTACH_PARENT_PROCESS = -1;

		public static bool IsHost { get { return Client.IsHost; } }

		[STAThread]
		public static void Main(string[] args)
		{
			LoggingServices.DefaultBackend = new Log4NetLoggingBackend();
			#region ConsoleAttach
			if (!(_consoleAttached = AttachConsole(ATTACH_PARENT_PROCESS)))
			{ Logger.Warn("Could not find Console to attach to."); }
#if DEBUG_CONSOLE
			if(!(_consoleAttached = AllocConsole()))
			{ Logger.Error("Console could not be created"); }
#endif
			if (!_consoleAttached)
			{ Logger.Info("continuing without console"); }
			if (_consoleAttached && SetConsoleMode(GetConsoleWindow(), 2))
			{ Logger.Warn("Could not set console mode"); }
			#endregion
			SetupScene setupScene = new SetupScene();
			Scene.RegisterScene(setupScene);
			sceneViewList.Add(typeof(SetupScene).FullName, new SetupView(setupScene));
			timer = new Timer() { Interval = 100 };
			timer.Elapsed += (s, e) =>
			{
				Client.Loop();
			};

			App app = new App();
			app.InitializeComponent();
			app.Run();
			FreeConsole();
		}
		public static void StartLocalServer()
		{
			Server = new Server(new LocalServerManager(IPAddress.Any));
			Server.Start();
			JoinDedicatedServer(IPAddress.Loopback, true);
		}
		public static void JoinDedicatedServer(IPAddress ip, bool isHost = false)
		{
			TcpClient tcpc = new TcpClient();
			tcpc.Connect(ip, 5801);
			Client = new Client(tcpc, isHost);
			timer.Start();
		}
		public static void Close()
		{
			if (Server != null)
				Server.Stop();
			if (Client != null)
				Client.Shutdown();
		}

		internal static View GetPage(Scene scene)
		{
			View view;
			if (!sceneViewList.TryGetValue(scene.GetType().FullName, out view))
			{ throw new UnknownPageException("No Page associated with Scene"); }
			return view;
		}
	}
}
