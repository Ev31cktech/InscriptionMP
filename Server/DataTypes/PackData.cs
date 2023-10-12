using Newtonsoft.Json.Linq;
using System.Linq;
using System.Reflection;

namespace Inscryption_Server.DataTypes
{
	public struct PackData
	{
		private JObject data;
		public string PackName { get; set; }
		public string Description { get; set; }
		public string Version { get; set; }
		public string GameVersion { get; set; }
		public string[] Authors { get; set; }
		public string SceneClass { get; set; }
		public string ViewClass { get; set; }
		public string AssemblyFile { get => Assembly.ManifestModule.Name; }
		public string AssemblyName { get => AssemblyFile.Replace(' ', '_').Split('.')[0]; }
		public Assembly Assembly { get; private set; }
		public JObject Content { get => data.Value<JObject>("Content"); }
		public string PackIdentifier { get => $"{PackName}({AssemblyFile})"; }

		public PackData(JObject data, Assembly assembly)
		{
			this.data = data;
			Assembly = assembly;
			PackName = "";
			Description = "";
			Version = "";
			GameVersion = "";
			Authors = null;
			SceneClass = "";
			ViewClass = "";
		}
		public override string ToString() => $"{PackIdentifier}";
	}
}
