using System;
using System.Windows;

namespace Inscription_mp
{
	public struct Settings
	{
		public String Username { get; set; }
		public Window window { get; set; }
		public struct Window
		{
			public WindowState State { get; set; }
			public uint Left { get; set; }
			public uint Top { get; set; }
			public uint Width { get; set; }
			public uint Height { get; set; }
			public Window()
			{
				State = WindowState.Normal;
				Left = 100;
				Top = 100;
				Width = 800;
				Height = 450;
			}
		}
		public Game game { get; set; }
		public struct Game
		{
			private Duration view_shift_duration;
			public Duration View_shift_duration { get { return view_shift_duration; } set { if (value.HasTimeSpan && value.TimeSpan.Ticks > 0) view_shift_duration = value; } }

			public Game()
			{
				View_shift_duration = new TimeSpan(0, 0, 0, 0, 200);
			}
		}
		public Settings()
		{
			window = new Window();
			game = new Game();
		}
	}
}
