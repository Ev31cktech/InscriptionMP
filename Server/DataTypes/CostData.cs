using Inscryption_Server.Classes;
using Inscryption_Server.Exceptions.PackExceptions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Inscryption_Server.DataTypes
{
	public struct CostData
	{
		private static Dictionary<string, CostType> costTypes = new Dictionary<string, CostType>();
		public CostType? Type { get; private set; }
		public uint Amount { get; }
		public CostData(string typeName, uint amount)
		{
			Amount = amount;
			Type = null;
			CostType data;
			if (amount > 0)
			{
				if (!costTypes.TryGetValue(typeName, out data))
				{ throw new CostException("unknown cost Type"); }
				Type = data;
			}
		}
		public CostData(JObject data) : this(data.Value<string>("Type"), data.Value<uint>("Amount")) { }
		public static void RegisterType(string type, string image)
		{
			costTypes.Add(type, new CostType(type,image));
		}
	}
}
