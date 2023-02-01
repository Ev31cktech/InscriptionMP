namespace Server.NetworkManagers
{
	public interface IServerManager
	{
		void Loop();
		void Shutdown();
	}
}