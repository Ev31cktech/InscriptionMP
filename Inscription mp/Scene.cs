using Inscription_mp.Support;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Inscription_mp
{
	public abstract class Scene
	{
		private int x = 0;
		private int y = 0;
		private Dictionary<Location, Page> views = new Dictionary<Location,Page>();
		private Storyboard MoveViewSTB = new Storyboard();
		public Scene(Page centerView)
		{
			View_Insert(centerView,0,0);
		}
		public void Scene_KeyDown(object Sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.W:
				case Key.Up:
					View_Up();
					break;
				case Key.A:
				case Key.Left:
					View_Left();
					break;
				case Key.S:
				case Key.Down:
					View_Down();
					break;
				case Key.D:
				case Key.Right:
					View_Right();
					break;
				default:
					break;
			}
		}

		private void View_Up()
		{
		}

		private void View_Left()
		{
		}

		private void View_Down()
		{
		}

		private void View_Right()
		{
		}

		public void View_Insert(Page view, int x, int y)
		{
			Location loc = new Location(x, y);
			if (views.ContainsKey(loc))
			{ views.Add(loc,view);}
			else
			{ views[loc] = view; }
		}
		public void View_SetActive(int x,int y)
		{
			MoveViewSTB.Children.Clear();
			MoveViewSTB.Children.Add(new DoubleAnimation(this.x,x,MainWindow.Settings.game.View_shift_duration));
			MoveViewSTB.Children.Add(new DoubleAnimation(this.y,y,MainWindow.Settings.game.View_shift_duration));
		}
	}
}
