using System.Windows.Controls;

namespace Inscription_mp
{
	public abstract class View : Page
	{
		protected View view;
		public View()
		{
			view = this;
		}
		public abstract void Update();
	}
}
