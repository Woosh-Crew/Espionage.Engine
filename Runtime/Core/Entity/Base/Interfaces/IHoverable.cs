namespace Espionage.Engine
{
	public interface IHoverable
	{
		/// <summary> What is the Title of this item. </summary>
		public string Title { get; }

		/// <summary> The bind to interact with this hoverable </summary>
		public string Action { get; }

		/// <summary> A Short Description of what this Hoverable does. </summary>
		public string Description { get; }

		bool Show( Pawn pawn );
	}
}
