﻿using Server.NetworkManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Timers;

namespace Server
{
	public class Server
	{
		static bool shutdown = false;
		static IServerManager server = null;
		static Timer looper = new Timer() { Interval = 100 };
		static Command[] commands = new Command[]
		{
			new Command(Command_Exit,"exit"),
			new Command(Command_Help,"help"),
			new Command(Command_Send,"send", "input"),
			new Command(Command_Start,"start", "IPAddress")
		};
		static void Main(string[] args)
		{
			looper.Elapsed += (s, e) => { if (server != null) server.Loop(); };
			try
			{
				while (!shutdown)
				{
					String inp = Console.ReadLine();
					if (!RunCommand(inp, commands))
					{
						Console.WriteLine("unknown command. Use help to see a list of commands");
					}
				}
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e.ToString());
			}
			Console.ReadKey();
		}

		static bool RunCommand(String inp, Command[] command)
		{
			for (int i = 0; i < commands.Length; i++)
			{
				List<string> args = inp.Split(' ').ToList();
				if (Regex.IsMatch(args[0], commands[i].command))
				{
					args.RemoveAt(0);
					if (!command[i].action.Invoke(args.ToArray()))
					{
						Console.WriteLine($"Incorrect use of command {inp.Split(' ')[0]}");
						if (command[i].arguments.Length > 0)
							Console.WriteLine($"Expected folowing arguments:\n{command[i].ListArguments()}");
					}
					return true;
				}
			}
			return false;
		}
		#region Commands

		static bool Command_Exit(String[] args)
		{
			Console.WriteLine("Shutting down...");
			looper.Stop();
			server.Shutdown();
			shutdown = true;
			return shutdown;
		}
		public static bool Command_Start(params String[] Args)
		{
			IPAddress ip = IPAddress.Any;
			if (Args.Length > 1 && IPAddress.TryParse(Args[0], out ip))
			{
				Console.WriteLine("Invalid IP");
				return false;
			}
			Console.WriteLine("Server started");
			server = new LocalServerManager(ip);
			looper.Start();
			return true;
		}
		public static bool Command_Help(String[] Args)
		{
			if (Args.Length < 1)
			{ Console.WriteLine("this is the list of commands that can be used:"); }
			foreach (Command cm in commands)
			{
				if (Args.Length > 0 && Args[0] != cm.command)
				{ continue; }
				{ Console.WriteLine($"- {cm.command}"); }
				if (cm.arguments.Length > 0)
				{ Console.WriteLine($"\t{cm.ListArguments()}"); }
			}
			return true;
		}
		static bool Command_Send(String[] args)
		{
			throw new NotImplementedException();
			return true;
		}
		#endregion
		/// <summary>
		/// The command object that holds the command name, decsription, 
		/// a list of all arguments(argument validation should be done by the action) and the action itself
		/// </summary>
		public struct Command
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
			{
				return String.Join("\n", arguments.Select(c => "\t-" + c));
			}
		}
	}
}