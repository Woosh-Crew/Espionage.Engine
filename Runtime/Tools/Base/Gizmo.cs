namespace Espionage.Engine.Tools
{
	public abstract class Gizmo : ILibrary
	{
		public Library ClassInfo { get; }

		public Gizmo()
		{
			ClassInfo = Library.Register( this );
		}

		public void Delete()
		{
			Library.Unregister( this );
		}
	}
}
