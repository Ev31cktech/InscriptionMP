using Inscryption_Server.Classes;
using Inscryption_Server.Exceptions.PackExceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using CostCardType = System.Tuple<Inscryption_Server.Classes.CostType, System.Type>;

namespace Inscryption_Server.DataTypes
{
	public struct CostData
	{
		private static Dictionary<string, CostCardType> costTypes = new Dictionary<string, CostCardType>();
		public CostType? Type { get; private set; }
		public uint Amount { get; }
		public CostData(string typeName, uint amount)
		{
			Amount = amount;
			Type = null;
			if (amount > 0)
			{
				CostCardType costType;
				if (!costTypes.TryGetValue(typeName, out costType))
				{ throw new CostException("unknown cost Type"); }
				Type = costType.Item1;
			}
		}
		public CostData(JObject data) : this(data.Value<string>("Type"), data.Value<uint>("Amount")) { }
		public static void RegisterType(string type, string imagePath, Type cardType)
		{
			costTypes.Add(type, new Tuple<CostType, Type>(new CostType(type, imagePath), cardType));
		}
		public static bool TryGetCostType(string input, out CostCardType cardType) => costTypes.TryGetValue(input, out cardType); 
	}
}
