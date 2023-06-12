using System.Reflection;

namespace Inscryption_Server.DataTypes
{
	public struct AssemblyData
	{
		public string PackName { get; set; }
		public string Description { get; set; }
		public string Version { get; set; }
		public string GameVersion { get; set; }
		public string[] Authors { get; set; }
		public string SceneClass { get; set; }
		public string ViewClass { get; set; }
		public string FullName { get => Assembly.FullName; }
		public Assembly Assembly { get; private set; }
		public AssemblyData(Assembly assembly)
		{
			Assembly = assembly;
			PackName = "";
			Description = "";
			Version = "";
			GameVersion = "";
			Authors = null;
			SceneClass = "";
			ViewClass = "";
		}
	}
}
