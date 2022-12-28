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
		private Dictionary<int, Dictionary<int, Page>> views = new Dictionary<int, Dictionary<int, Page>>();
		private Storyboard MoveViewSTB = new Storyboard();
		public Scene(Page centerView)
		{
			views[x][y] = centerView;
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
			if (views[x] == null)
				views[x] = new Dictionary<int, Page>();
			views[x][y] = view;
		}
		public void View_SetActive(int x,int y)
		{
			MoveViewSTB.Children.Clear();
			MoveViewSTB.Children.Add(new DoubleAnimation(this.x,x,MainWindow.Settings.game.View_shift_duration));
			MoveViewSTB.Children.Add(new DoubleAnimation(this.y,y,MainWindow.Settings.game.View_shift_duration));
		}
	}
}
