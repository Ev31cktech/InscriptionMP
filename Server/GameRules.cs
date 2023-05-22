namespace Inscription_Server
{
	public class GameRules
	{
		public BoardRules Board { get; set; } = new BoardRules();
		public class BoardRules
		{
			public uint ColumnCount { get; set; }
		}
	}
}
