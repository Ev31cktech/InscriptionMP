using Inscription_Server;
using Inscription_Server.NetworkManagers;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;


namespace Inscription_mp
{
	public class App : Application
	{
		private static Server server;
		public static Client client;
		public static Timer timer;
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


		[STAThread]
		public static void Main(string[] args)
		{
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

			timer= new Timer() {Interval = 20};
			timer.Elapsed += (s,e) => {
				client.AddData(JObject.Parse("{ \"test\" : \"test\"}"));
				client.Loop();
			};

			App app = new App();
			app.InitializeComponent();
			app.Run();
			FreeConsole();
		}
		public static void StartLocalServer()
		{
			server = new Server(new LocalServerManager(server,IPAddress.Any));
			server.Start();
			JoinDedicatedServer(IPAddress.Any);
		}
		public static void JoinDedicatedServer(IPAddress ip)
		{
			TcpClient tcpc = new TcpClient();
			tcpc.Connect(IPAddress.Loopback,5801);
			client = new Client(tcpc,true);
		}
		public static void Close()
		{
			if(server != null)
			server.Stop();
			if(client != null)
			client.Shutdown();
		}
	}
}
