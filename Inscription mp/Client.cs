using Newtonsoft.Json.Linq;
using System.Net.Sockets;

namespace Inscription_mp
{
	public class Client : Inscription_Server.Client
	{
		public View CurrentView { get; private set; }
		public Client(TcpClient socket, bool IsHost = false) : base(socket,IsHost)
		{ }
		public override void ChangeScene(Inscription_Server.Client client,JObject data)
		{
			base.ChangeScene(client,data);
			CurrentView = App.GetPage(CurrentScene);
			MainWindow.MainWindow_ShowView(CurrentView);
		}
		public override void RunAction(string func, JObject data)
		{
			base.RunAction(func, data);
			CurrentView.Update();
		}
	}
}
