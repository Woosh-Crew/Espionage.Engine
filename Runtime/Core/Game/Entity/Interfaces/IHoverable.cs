namespace Espionage.Engine
{
	public interface IHoverable
	{
		public string Name { get; }
		public string Action { get; }
		public string Description { get; }

		bool Show( Pawn pawn );
	}
}
