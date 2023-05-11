using Inscription_Server;
using System.Windows.Controls;

namespace Inscription_mp
{
	
	public abstract class View<T>  : View where T : Scene
	{
		public T thisScene{ get; private set; }
		public View<T> thisView { get; private set; }
		public View( T scene)
		{
			DataContext = scene;
			thisScene = scene;
			thisView = this;
		}
	}
	public abstract class View : UserControl
	{}
}
