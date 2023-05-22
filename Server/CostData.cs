namespace Inscription_Server
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
