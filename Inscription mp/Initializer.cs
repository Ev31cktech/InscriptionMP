using Inscryption_Server;
using Inscryption_Server.DataTypes;
using Inscryption_Server.Exceptions;
using Inscryption_Server.Exceptions.PackExceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Inscryption_mp
{
	internal class Initializer : Inscryption_Server.Initializer
	{
		public void Initialize()
		{
			PackData pack = new PackData();
			if (!CheckAssembly(Assembly.GetAssembly(typeof(Initializer)), ref pack))
				throw new PackException();
			packs.Add(pack);
			base.Initialize();
		}
		/*
		protected override void InitializeCardData()
		{
			List<PropertyInfo> properties = new List<PropertyInfo>();
			properties.AddRange(typeof(CardData).GetProperties().Where(prop => !typeof(Inscryption_Server.DataTypes.CardData).GetProperties().Select(p => p.Name).Contains(prop.Name)));
			packs.ForEach(pack => InitializeCardData(pack, ref properties));
			cardType = DynamicTypeCreator.ExpandType($"Full{nameof(CardData)}", typeof(CardData), properties.ToArray());
		}
		*/
		protected override void InitializeCard(JObject data, int cardIndex, ref CardData card, PackData packData)
		{
			string portraitPath = data.Value<string>("Portrait");
			string portraitLoc = $"{packData.AssemblyName}{portraitPath.Replace('/', '.')}";
			if (!data.ContainsKey("Portrait"))
			{ throw new CardException($"Card {cardIndex} in pack {packData.PackIdentifier} did not contain a 'Portrait'."); }
			if (!packData.Assembly.GetManifestResourceNames().Contains(portraitLoc))
			{ throw new CardException($"Card {cardIndex} in pack {packData.PackIdentifier} did not contain a matching file with {portraitPath}. make sure you have the file included in the dll by setting it's build action to 'Resource'"); }
			UIElement xaml = XamlReader.Load(packData.Assembly.GetManifestResourceStream(portraitLoc)) as UIElement;
			(card as Inscryption_mp.DataTypes.CardData).Portrait = xaml;
			base.InitializeCard(data, cardIndex, ref card, packData);
		}
		protected override void InitializeScenes(JArray scenesData, PackData assemblyData)
		{
			for (int i = 0; i < scenesData.Count; i++)
			{
				string sceneClass = scenesData[i].Value<string>("SceneClass");
				string viewClass = scenesData[i].Value<string>("ViewClass");
				if (sceneClass == null)
				{ throw new InvalidAssemblyException($"Scene {i} in {assemblyData.PackIdentifier} did not contain a 'SceneClass' attribute."); }
				else if (viewClass == null)
				{ throw new InvalidAssemblyException($"Scene {i} in {assemblyData.PackIdentifier} did not contain a 'ViewClass' attribute."); }
				Scene scene = RegisterScene(sceneClass, assemblyData);
				if (scene != null)
				{
					Assembly assembly = assemblyData.Assembly;
					if (sceneClass.StartsWith("Inscryption_Server"))
						assembly = Assembly.GetExecutingAssembly();
					try
					{
						Type viewType = assembly.GetType(viewClass);
						if (viewType.GetConstructor(new Type[] { scene.GetType() }) == null)
						{ throw new SceneException($"View {viewType.Name} did not contain a valid public Constructor. Take a look at the documentation at https://github.com/Ev31cktech/InscriptionMP/wiki to see what is needed for your view to be used"); }
						View view = Activator.CreateInstance(viewType, scene) as View;
						View.RegisterView(sceneClass, view);
					}
					catch (SceneException e)
					{
						App.Logger.Error(e.Message);
					}
					catch (Exception e)
					{
						App.Logger.Error($"Scene {i} in {assemblyData.PackIdentifier} 'ViewClass' attribute is invalid.");
						App.Logger.Debug(e);
					}
				}
			}
		}
	}
}
