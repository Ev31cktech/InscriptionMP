using Inscription_mp.Exceptions;
using Inscription_mp.Views;
using Inscription_Server;
using Inscription_Server.NetworkManagers;
using Inscription_Server.Scenes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Inscription_mp
{
	public class App : Application
	{
		private static Server server;
		private static Client client;
		private static Timer timer;
		private static Dictionary<string, Page> scenePageList = new Dictionary<string, Page>();
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

		public static bool IsHost { get{return client.IsHost; } }

		[STAThread]
		public static void Main(string[] args)
		{
			#region ConsoleAttach
			if (!(_consoleAttached = AttachConsole(ATTACH_PARENT_PROCESS)))
			{ Console.WriteLine("Could not find Console to attach to.");}
#if DEBUG_CONSOLE
			if(!(_consoleAttached = AllocConsole()))
			{ Console.WriteLine("Console could not be created"); }
#endif
			if(!_consoleAttached)
			{Console.WriteLine("continuing without console");}
			if(_consoleAttached && SetConsoleMode(GetConsoleWindow(),2))
			{ Console.WriteLine("Could not set console mode");}
#endregion
			Scene.RegisterScene(new SetupScene());
			scenePageList.Add(typeof(SetupScene).FullName, new SetupView(null));
			timer = new Timer() {Interval = 100};
			timer.Elapsed += (s,e) => {
				client.Loop();
			};

			App app = new App();
			app.InitializeComponent();
			app.Run();
			FreeConsole();
		}
		public static void StartLocalServer()
		{
			server = new Server(new LocalServerManager(IPAddress.Any));
			server.Start();
			JoinDedicatedServer(IPAddress.Loopback,true);
		}
		public static void JoinDedicatedServer(IPAddress ip, bool isHost = false)
		{
			TcpClient tcpc = new TcpClient();
			tcpc.Connect(ip,5801);
			client = new Client(tcpc,isHost);
			timer.Start();
		}
		public static void Close()
		{
			if(server != null)
			server.Stop();
			if(client != null)
			client.Shutdown();
		}

		internal static Page GetPage(Scene scene)
		{
			Page page;
			if(!scenePageList.TryGetValue(scene.GetType().FullName,out page))
			{throw new UnknownPageException("No Page associated with Scene"); }
			return page;
		}
	}
}
