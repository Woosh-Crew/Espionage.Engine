namespace Espionage.Engine.Logic
{
	public class Button : Entity, IUsable, IHoverable
	{
		public bool OnUse( Pawn user )
		{
			Dev.Log.Info( "Used Button" );
			return true;
		}

		public bool IsUsable( Pawn user )
		{
			return true;
		}

		// Hoverable

		public virtual string Title => ClassInfo.Title;
		public string Action => "[F]";
		public string Description => "Press Button";

		bool IHoverable.Show( Pawn pawn )
		{
			return true;
		}
	}
}
