using Inscription_Server;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Windows.Controls;

namespace Inscription_mp
{
	public class Client : Inscription_Server.Client
	{
		public Client(TcpClient socket, bool IsHost = false) :base(socket,IsHost)
		{ }
		public override void ChangeScene(JObject data)
		{
			base.ChangeScene(data);
			Page page = App.GetPage(CurrentScene);
			MainWindow.MainWindow_ShowView(page);
		}
	}
}
