using Inscryption_Server.Classes;
using Inscryption_Server.DataTypes;
using Inscryption_Server.Exceptions;
using Inscryption_Server.Exceptions.PackExceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Inscryption_Server
{
	internal class Initializer
	{
		const string ContentPath = "./Content";
		protected List<PackData> packs = new List<PackData>();
		//protected Type cardType = typeof(CardData);

		public virtual void Initialize()
		{
			PackData pack = new PackData();
			if (!CheckAssembly(Assembly.GetAssembly(typeof(Initializer)), ref pack))
				throw new PackException();
			packs.Add(pack);
			InitializeMods();
		}
		private void InitializeMods()
		{
			Directory.CreateDirectory(ContentPath);
			string[] modAssemblies = Directory.GetFiles(ContentPath);
			foreach (string assemblyPath in modAssemblies)
			{
				Assembly assembly;
				try
				{
					PackData pack = new PackData();
					assembly = Assembly.LoadFile(Path.GetFullPath(assemblyPath));
					pack = new PackData();
					if (!CheckAssembly(assembly, ref pack))
						continue;
					packs.Add(pack);
				}
				catch (Exception e)
				{ App.Logger.Error(e); }
			}
			App.Logger.Info("Started initializing CostTypes");
			packs.ForEach(pack => InitializeCostTypes(pack.Content, pack));
			//List<PropertyInfo> properties = new List<PropertyInfo>();
			//packs.ForEach(pack => InitializeCardData(pack, ref properties));
			packs.ForEach(pack => InitializeCardTypes(pack.Content, pack));
			packs.ForEach(pack => InitializeCards(pack.Content, pack));
			packs.ForEach(pack => InitializeScenes(pack.Content, pack));
		}
		internal bool CheckAssembly(Assembly assembly, ref PackData pack)
		{
			pack = new PackData();
			List<string> resources = assembly.GetManifestResourceNames().ToList();
			string metaPath = resources.Find((i) => i.EndsWith("MetaData.json"));
			if (metaPath == null || metaPath == "")
			{ return false; }
			using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(metaPath)))
			{
				JObject root = JObject.Parse(sr.ReadToEnd());
				pack = new PackData(root, assembly);
				InitializeAssembly(root, assembly, ref pack);
			}
			return true;
		}
		private void InitializeAssembly(JObject data, Assembly assembly, ref PackData packData)
		{
			packData.PackName = data.Value<string>("Name");
			packData.Authors = data.Value<JArray>("Authors").Values<string>().ToArray();
			packData.Version = data.Value<string>("Version");
			packData.GameVersion = data.Value<string>("GameVersion");
			if (packData.PackName == null)
			{ throw new InvalidAssemblyException($"assembly: {packData.AssemblyName} does not contain a 'Name'."); }
			if (packData.Authors == null)
			{ throw new InvalidAssemblyException($"assembly: {packData.PackIdentifier} does not contain an 'Author'."); }
			if (packData.Version == null)
			{ throw new InvalidAssemblyException($"assembly: {packData.PackIdentifier} does not contain a 'Version'."); }
			if (packData.GameVersion == null)
			{ throw new InvalidAssemblyException($"assembly: {packData.PackIdentifier} does not target a 'GameVersion'."); }
		}
		private void InitializeCostTypes(JObject data, PackData packData)
		{
			App.Logger.Debug($"Initializing cost for {packData.PackName}");
			JToken token;
			if (data.TryGetValue("Costs", out token))
			{
				JArray costsData = token.Value<JArray>();
				InitializeCosts(costsData, packData);
			}
		}

		private void InitializeCosts(JArray costsData, PackData packData)
		{
			for (int i = 0; i < costsData.Count; i++)
			{
				try
				{

				}
				catch (Exception)
				{ }
			}
		}

		private void InitializeCards(JObject data, PackData packData)
		{
			App.Logger.Debug($"Initializing cards for {packData.PackName}");
			JToken token;

			if (data.TryGetValue("Cards", out token))
			{
				JArray cardsData = token.Value<JArray>();
				InitializeCards(cardsData, packData);
			}
		}
		protected void InitializeCards(JArray cardData, PackData packData)
		{
			for (int i = 0; i < cardData.Count; i++)
			{
				Type cardType = packData.Assembly.GetType();
				CardData card = Activator.CreateInstance(cardType) as CardData;
				try
				{
					InitializeCard(cardData[i] as JObject, i, ref card, packData);
					Card.RegisterCard(card);
				}
				catch (CardException e)
				{
					App.Logger.Error(e.Message);
					App.Logger.Debug(e.StackTrace);
				}
				catch { }
			}
		}
		protected virtual void InitializeCard(JObject data, int cardIndex, ref CardData card, PackData packData)
		{
			if (!data.ContainsKey("Name"))
			{ throw new CardException($"Card {cardIndex} in pack {packData.PackIdentifier} did not contain a 'Name'."); }
			string name = data.Value<string>("Name").Replace(' ', '_');

			if (!data.ContainsKey("Health"))
			{ throw new CardException($"Card {cardIndex} in pack {packData.PackIdentifier} did not contain 'Health'."); }
			uint health = data.Value<uint>("Health");

			if (!data.ContainsKey("Power"))
			{ throw new CardException($"Card {cardIndex} in pack {packData.PackIdentifier} did not contain 'Power'."); }
			uint power = data.Value<uint>("Power");

			Sigil[] sigils = Sigil.GetSigils(data.Value<string[]>("Sigils"));
			string species = data.Value<string>("Species"); //TODO probably needs to change to a class to hold the image data needed.

			if (data.SelectToken("Cost.Type") == null)
			{ throw new CardException($"Card {cardIndex} in pack {packData.PackIdentifier} did not contain 'Cost.Type'."); }
			if (data.SelectToken("Cost.Amount") == null)
			{ throw new CardException($"Card {cardIndex} in pack {packData.PackIdentifier} did not contain 'Cost.Amount'."); }
			CostData cost = new CostData(data.Value<JObject>("Cost"));

			card.Initialize($"{packData.PackName}.{name}", name, health, power, cost, sigils, species);
		}

		private void InitializeScenes(JObject data, PackData packData)
		{
			App.Logger.Debug($"Initializing scenes for {packData.PackName}");
			JToken token;
			if (data.TryGetValue("Scenes", out token))
			{
				JArray scenesData = token.Value<JArray>();
				InitializeScenes(scenesData, packData);
			}
		}
		protected virtual void InitializeScenes(JArray scenesData, PackData packData)
		{
			for (int i = 0; i < scenesData.Count; i++)
			{
				string cardTypeName = scenesData[i].Value<string>("Name");
				string cardTypeClass = scenesData[i].Value<string>("Type");
				if (cardTypeName == null)
				{ throw new InvalidAssemblyException($"cardType[{i}] in packData:  did not contain a 'Name' attribute."); }
				//TODO import image, maybe rewrite this part.
				CostData.RegisterType(cardTypeName,"",packData.Assembly.GetType(cardTypeClass));
			}
		}
		private void InitializeCardTypes(JObject data, PackData packData)
		{
			App.Logger.Debug($"Initializing CardData for {packData.PackName}");
			JToken token;
			if (data.TryGetValue("CardTypes", out token))
			{
				JArray scenesData = token.Value<JArray>();
				InitializeCardTypes(scenesData, packData);
			}
		}
		private void InitializeCardTypes(JArray cardTypedata, PackData packData)
		{
			for (int i = 0; i < cardTypedata.Count; i++)
			{
				string sceneClass = cardTypedata[i].Value<string>("SceneClass");
				if (sceneClass == null)
				{ throw new InvalidAssemblyException($"Cardtype[{i}] in packData:  did not contain a 'SceneClass' attribute."); }
				Card(sceneClass, packData);
			}
		}
		/*
		protected void InitializeCardData(PackData packData, ref List<PropertyInfo> properties)
		{
			foreach (Type CardDataType in packData.Assembly.ExportedTypes.Where(t => t.IsSubclassOf(typeof(CardData))))
			{ properties.AddRange(CardDataType.GetProperties().Where(prop => !typeof(CardData).GetProperties().Select(p => p.Name).Contains(prop.Name))); }
		}
		*/

		protected Scene RegisterScene(string name, PackData packData)
		{
			Scene scene;
			try
			{
				Type sceneType = packData.Assembly.GetTypes().First(t => t.FullName == name);
				if (sceneType.GetConstructor(new Type[0]) == null)
					throw new SceneException($"Scene {name} did not contain a public Constructor with 0 paramaters. Make sure it has one to be able to be used");
				scene = Activator.CreateInstance(sceneType) as Scene;
				Scene.RegisterScene(scene);
				return scene;
			}
			catch (Exception)
			{
				{ App.Logger.Error($"Scene with ScenePath {name} in pack: {packData.PackIdentifier} is Invalid"); }
				return null;
			}
		}
	}
}
