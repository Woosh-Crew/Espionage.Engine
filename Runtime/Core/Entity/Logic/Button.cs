namespace Espionage.Engine.Logic
{
	public class Button : Entity, IUsable, IHoverable
	{
		public virtual bool IsUsable( Pawn user )
		{
			return true;
		}

		public virtual bool OnUse( Pawn user )
		{
			Debugging.Log.Info( "Used Button" );
			return true;
		}

		public virtual void Started( Pawn user ) { }
		public virtual void Stopped( Pawn user ) { }

		[Function, Callback( "app.frame" )]
		private void Frame()
		{
			Overlay.Box( Position, Scale, depthTest : true );
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
