﻿namespace Inscryption_Server.DataTypes
{
	public struct CostData
	{
		public string Type {get;private set;}
		public CostData(string type)
		{
			Type = type;
		}
	}
}
