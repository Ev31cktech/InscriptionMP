using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Inscryption_Server.NetworkManagers;
using Inscryption_Server.Scenes;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;
using PostSharp.Patterns.Diagnostics.Backends.Log4Net;

[assembly: Log]
[assembly: Log(AttributePriority = 1)]
//[assembly: Log(AttributeTargetMembers = "regex:^Loop", AttributeExclude = true, AttributePriority = 2)]
//[assembly: Log(AttributeTargetMembers = "regex:^..ctor", AttributeExclude = true, AttributePriority = 2)]
//[assembly: Log(AttributeTargetMembers = "regex:^get_|^set_", AttributeExclude = true, AttributePriority = 3)]
[assembly: InternalsVisibleTo("Inscryption mp")]

namespace Inscryption_Server
{
	internal class App
	{
		private static Server server;
		private static bool shutdown = false;
		private static Initializer initializer = new Initializer();
		public static Server Server { get => Server.ThisServer; }
		public static ILog Logger { get { return LogManager.GetLogger("SERVER"); } }

		private static Command[] commands = new Command[]
		{
			new Command(Command_Exit,"exit"),
			new Command(Command_Help,"help"),
			new Command(Command_Send,"send", "input"),
			new Command(Command_Start,"start", "IPAddress"),
			new Command(Command_Stop,"stop"),
			new Command(Command_Crash,"crash")
		};
		internal static void Main(string[] args)
		{
			initializer.Initialize();
			LoggingServices.DefaultBackend = new ConsoleLoggingBackend();//InitializeBackend();
			Logger.Info("Server Logging Enabled");
			Stack<string> argStack = new Stack<string>(args);
			Scene.RegisterScene(new SetupScene());
			if (argStack.Count > 1)
			{ RunCommand(String.Join(" ", argStack), commands); }
			try
			{
				while (!shutdown)
				{
					String inp = Console.ReadLine();
					if (!RunCommand(inp, commands))
					{ Logger.Warn("unknown command. Use help to see a list of commands"); }
				}
				Console.WriteLine("Press any key to close the console");
			}
			catch (Exception e)
			{ Console.Error.WriteLine(e.ToString()); }
			Console.ReadKey();
		}


		internal static Log4NetLoggingBackend InitializeBackend()
		{
			var repo = (Hierarchy)LogManager.GetRepository();
#if DEBUG
			repo.Root.Level = log4net.Core.Level.All;
#endif
			//XmlConfigurator.Configure(repo);
			XmlConfigurator.Configure(new FileInfo("logger.config"));
			return new Log4NetLoggingBackend();
		}

		private static bool RunCommand(String inp, Command[] commands)
		{
			for (int i = 0; i < commands.Length; i++)
			{
				List<string> args = inp.Split(' ').ToList();
				if (Regex.IsMatch(args[0], commands[i].command))
				{
					args.RemoveAt(0);
					if (!commands[i].action.Invoke(args.ToArray()))
					{
						Logger.Error($"Incorrect use of command {inp.Split(' ')[0]}");
						if (commands[i].arguments.Length > 0)
							Logger.Info($"Expected folowing arguments:\n{commands[i].ListArguments()}");
					}
					return true;
				}
			}
			return false;
		}

		#region Commands
		private static bool Command_Crash(String[] args)
		{
			throw new Exception("this is a test command to crash the program");
		}

		private static bool Command_Exit(String[] args)
		{
			server.Stop();
			shutdown = true;
			return shutdown;
		}

		private static bool Command_Start(params String[] Args)
		{
			IPAddress ip = IPAddress.Loopback;
			if (Args.Length > 1 && IPAddress.TryParse(Args[0], out ip))
			{
				Logger.Error("Invalid IP");
				return true;
			}
			if (server == null || !server.IsAllive)
				server = new LocalServer(ip);
			if (server.IsAllive)
			{ Logger.Warn("Server is already running"); return true; }
			server.Start();
			return true;
		}

		private static bool Command_Stop(params String[] Args)
		{
			server.Stop();
			return true;
		}

		private static bool Command_Help(String[] Args)
		{
			if (Args.Length < 1)
			{ Logger.Info("this is the list of commands that can be used:"); }
			foreach (Command cm in commands)
			{
				if (Args.Length > 0 && Args[0] != cm.command)
				{ continue; }
				{ Logger.Info($"- {cm.command}"); }
				if (cm.arguments.Length > 0)
				{ Logger.Info($"\t{cm.ListArguments()}"); }
			}
			return true;
		}
		private static bool Command_Send(String[] args)
		{ return true; }
		#endregion
		/// <summary>
		/// The command object that holds the command name, decsription, 
		/// a list of all arguments(argument validation should be done by the action) and the action itself
		/// </summary>
		private struct Command
		{
			public Func<String[], bool> action { get; private set; }
			public String command { get; private set; }
			public String[] arguments { get; private set; }
			public Command(Func<String[], bool> _action, String _command, params String[] _arguments)
			{
				action = _action;
				command = _command;
				arguments = _arguments;
			}
			public String ListArguments()
			{ return String.Join("\n", arguments.Select(c => "\t-" + c)); }
		}
	}
}