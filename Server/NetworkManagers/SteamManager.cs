using Steamworks;
using System;

namespace Server.NetworkManagers
{
	public class SteamManager : IServerManager
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
			Console.WriteLine();
		}
		public void Loop()
		{
			SteamAPI.RunCallbacks();
		}

		public void Shutdown()
		{
		}

	}
}
