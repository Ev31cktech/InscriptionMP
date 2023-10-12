#define DEBUG_CONSOLE

using Inscryption_Server.NetworkManagers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Runtime.InteropServices;
using log4net;
using PostSharp.Patterns.Diagnostics;
using Newtonsoft.Json;
using System.Threading;
using System.Reflection;

[assembly: Log]
[assembly: Log(AttributeExclude = true, AttributeTargetMembers = "regex:^get_|^set_|^Loop", AttributePriority = 3)]
[assembly: Log(AttributeExclude = true, AttributeTargetMembers = "regex:\\.\\.ctor\\(\\)$")]
namespace Inscryption_mp
{
	public class App : Application
	{
		private static App app { get; } = new App();
		public static Settings Settings
		{
			get
			{
				if (Inscryption_mp.Properties.Settings.Default.AllSettings != "")
				{ app.settings = JsonConvert.DeserializeObject<Settings>(Inscryption_mp.Properties.Settings.Default.AllSettings); }
				return app.settings;
			}
			set
			{
				app.settings = value;
				Inscryption_mp.Properties.Settings.Default.AllSettings = JsonConvert.SerializeObject(value);
				Inscryption_mp.Properties.Settings.Default.Save();
			}
		}
		private static Server server;
		internal static Client Client { get; private set; }
		public static ILog Logger { get { return LogManager.GetLogger("CLIENT"); } }
		private static Timer looper;
		internal static Initializer Initializer { get; } = new Initializer();
		private Settings settings;
		private bool _contentLoaded;
		private static bool looping = false;
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


		[DllImport("kernel32.dll")]
		static extern bool AttachConsole(int dwProcessId);
		[DllImport("kernel32.dll")]
		static extern bool FreeConsole();
		private const int ATTACH_PARENT_PROCESS = -1;


		[STAThread]
		internal static void Main(string[] args)
		{
			if (!(_consoleAttached = AttachConsole(ATTACH_PARENT_PROCESS)))
			{ Logger.Warn("Could not find Console to attach to."); }

			LoggingServices.DefaultBackend = Inscryption_Server.App.InitializeBackend();
			//SetupScene setupScene = new SetupScene();
			//Scene.RegisterScene(setupScene);
			//BoardScene boardScene = new BoardScene(new JObject());
			//Scene.RegisterScene(boardScene);
			//sceneViewList.Add(typeof(SetupScene).FullName, new SetupView(setupScene));
			//sceneViewList.Add(typeof(BoardScene).FullName, new BoardView(boardScene));

			Assembly.GetExecutingAssembly();
			app.Resources.MergedDictionaries.Add(Application.LoadComponent(new Uri("/AppRecourceDictionary.xaml", UriKind.Relative)) as ResourceDictionary);
			app.InitializeComponent();
			app.Run();
			FreeConsole();
			Close();
		}
		internal static void StartLocalServer()
		{
			server = new LocalServer(IPAddress.Any);
			server.Start();
			JoinDedicatedServer(IPAddress.Loopback);
		}
		internal static void JoinDedicatedServer(IPAddress ip)
		{
			TcpClient tcpc = new TcpClient();
			try
			{
				tcpc.Connect(ip, 5801);
				Client = new Client(tcpc);
				looper = new Timer(app.Loop, null, 0, 1);
			}
			catch (SocketException e)
			{
				Logger.Error($"Could not connect. Are you a server is running on that IP?\n{e.Message}");
				Inscryption_mp.MainWindow.ShowMainView();
			}
			catch (Exception e)
			{
				Logger.Error($"{e.Message}\n\n{e.StackTrace}");
				Inscryption_mp.MainWindow.ShowMainView();
			}
		}
		internal void Loop(object sender)
		{
			if (!looping)
			{
				try
				{
					looping = true;
					if (Client.Connected)
						Client.Loop();
				}
				catch (Exception e)
				{
					Logger.Error(e.Message, e);
					MessageBox.Show($"{e.Message}\n\n{e.StackTrace}"); //TODO do not use Messagebox
				}
				finally
				{
					looping = false;
				}
			}
		}
		internal static void Close()
		{
			if (server != null)
				server.Stop();
			if (looper != null)
				looper.Dispose();
			if (Client != null)
				Client.Shutdown();
			app.Shutdown();
		}
	}
}
