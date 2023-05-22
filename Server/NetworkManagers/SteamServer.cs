using Steamworks;

namespace Inscription_Server.NetworkManagers
{
	public class SteamServer : Server
	{
		public string user
		{
			get
			{
				if (!Inited) return null;
				return SteamFriends.GetPersonaName();
			}
		}
		public bool Inited { get; private set; }
		public void Init()
		{
			Inited = SteamAPI.Init();
		}
		public override void Loop()
		{
			SteamAPI.RunCallbacks();
		}

		public override void Shutdown()
		{}

	}
}
