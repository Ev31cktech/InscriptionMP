using Inscryption_mp.Exceptions;
using Inscryption_Server;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Inscryption_mp
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
	{
		private static Dictionary<string, View> sceneViewList = new Dictionary<string, View>();
		public View()
		{ Focusable = true; }
		public abstract void Initialize();
		internal static View GetView(Scene scene)
		{
			View view;
			if (!sceneViewList.TryGetValue(scene.GetType().FullName, out view))
			{ throw new UnknownViewException("No view associated with Scene"); }
			view.Initialize();
			return view;
		}
		internal static void RegisterView(string sceneClass ,View view)
		{
			sceneViewList.Add(sceneClass, view);
		}
	}
}
