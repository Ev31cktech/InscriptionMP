namespace Server
{
	public struct Cost
	{
		public string Type {get;private set;}
		public Cost(string type)
		{
			Type = type;
		}
	}
}
