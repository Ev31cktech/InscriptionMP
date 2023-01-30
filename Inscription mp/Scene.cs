using Inscription_mp.Support;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Inscription_mp
{
	public abstract class Scene
	{
		private int x = 0;
		private int y = 0;
		private Dictionary<Location, Page> views = new Dictionary<Location, Page>();
		private Storyboard MoveViewSTB = new Storyboard();
		public Page CurrentView { get { return views[new Location(x, y)]; } }

		public Scene(Page centerView)
		{
			View_Insert(centerView, 0, 0);
		}
		public virtual void Scene_KeyDown(object Sender, KeyEventArgs e)
		{
		}

		public void View_Insert(Page view, int x, int y)
		{
			Location loc = new Location(x, y);
			if (views.ContainsKey(loc))
			{ views.Add(loc, view); }
			else
			{ views[loc] = view; }
		}
		public Page getView(int x,int  y)
		{
			return views[new Location(x,y)];
		}
	}
}
