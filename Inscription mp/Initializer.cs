using Inscryption_Server;
using Inscryption_Server.DataTypes;
using Inscryption_Server.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Inscryption_mp
{
	public class Initializer : Inscryption_Server.Initializer
	{
		protected override void InitializeScenes(JArray scenesData, AssemblyData assemblyData)
		{
			for (int i = 0; i < scenesData.Count; i++)
			{
				string sceneClass = scenesData[i].Value<string>("SceneClass");
				string viewClass = scenesData[i].Value<string>("ViewClass");
				if (sceneClass == null)
				{ throw new InvalidAssemblyException($"Scene {i} in {assemblyData.FullName} did not contain a 'SceneClass' attribute."); }
				else if (viewClass == null)
				{ throw new InvalidAssemblyException($"Scene {i} in {assemblyData.FullName} did not contain a 'ViewClass' attribute."); }
				Scene scene = RegisterScene(sceneClass, assemblyData);
				if (scene != null)
				{
					Assembly assembly = assemblyData.Assembly;
					if (sceneClass.StartsWith("Inscryption_Server"))
						assembly = Assembly.GetExecutingAssembly();
					try
					{
						Type viewType = assembly.GetType(viewClass);
						View view = Activator.CreateInstance(viewType, scene) as View;
						View.RegisterView(sceneClass, view);
					}
					catch (Exception)
					{
						App.Logger.Error($"Scene {i} in {assemblyData.FullName} 'ViewClass' attribute is invalid.");
					}
				}
			}
		}
	}
}
