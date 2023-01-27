using Steamworks;

namespace Inscription_mp.Network
{
	class SteamManager
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
		public SteamManager()
		{
			Inited = SteamAPI.Init();
		}
	}
}
