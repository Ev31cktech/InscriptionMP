using Inscryption_Server.DataTypes;
using Inscryption_Server.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static Inscryption_Server.Scene;

namespace Inscryption_Server
{
	public class Initializer
	{
		public void InitalizeAssembly(Assembly assembly)
		{
			string[] resources = assembly.GetManifestResourceNames();
			for (int i = 0; i < resources.Length; i++)
			{
				if (resources[i].EndsWith("MetaData.json"))
				{

					using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(resources[i])))
					{ InitializeAssembly(JObject.Parse(sr.ReadToEnd()), new AssemblyData(assembly)); }
				}
				if (i == resources.Length)
				{ throw new InvalidAssemblyException(); }
			}
		}
		private void InitializeAssembly(JObject data, AssemblyData assembly)
		{
			assembly.PackName = data.Value<string>("Name");
			assembly.Authors = data.Value<JArray>("Authors").Values<string>().ToArray();
			assembly.Version = data.Value<string>("Version");
			assembly.GameVersion = data.Value<string>("GameVersion");
			if (assembly.PackName == null)
			{ throw new InvalidAssemblyException($"assembly: {assembly.FullName} does not contain a 'Name'."); }
			if (assembly.Authors == null)
			{ throw new InvalidAssemblyException($"assembly: {assembly.PackName}({assembly.FullName}) does not contain an 'Author'."); }
			if (assembly.Version == null)
			{ throw new InvalidAssemblyException($"assembly: {assembly.PackName}({assembly.FullName}) does not contain a 'Version'."); }
			if (assembly.GameVersion == null)
			{ throw new InvalidAssemblyException($"assembly: {assembly.PackName}({assembly.FullName}) does not target a 'GameVersion'."); }

			JArray scenesData;
			JToken token;
			if (data.TryGetValue("Scenes", out token))
			{
				scenesData = token.Value<JArray>();
				InitializeScenes(scenesData, assembly);
			}
		}
		protected virtual void InitializeScenes(JArray scenesData, AssemblyData assembly)
		{
			for (int i = 0; i < scenesData.Count; i++)
			{
				string sceneClass = scenesData[i].Value<string>("SceneClass");
				if (sceneClass == null)
				{ throw new InvalidAssemblyException($"Scene[{i}] in assembly: {assembly.PackName}({assembly.FullName}) did not contain a 'SceneClass' attribute."); }
				RegisterScene(sceneClass, assembly);
			}
		}
		protected Scene RegisterScene(string name, AssemblyData assembly)
		{
			Scene scene;
			try
			{
				scene = assembly.Assembly.CreateInstance(name) as Scene;
				Scene.RegisterScene(scene);
				return scene;
			}
			catch (Exception)
			{

				{ App.Logger.Error($"Scene with ScenePath {name} in assembly: {assembly.PackName}({assembly.FullName}) is Invalid"); }
				return null;
			}
		}
	}
}
