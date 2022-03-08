namespace Espionage.Engine.Mod
{
	[Spawnable, Group( "Engine" )]
	public abstract class Mod : IPackage
	{
		public Library ClassInfo { get; }

		protected Mod()
		{
			ClassInfo = Library.Register( this );
		}

		~Mod()
		{
			Library.Unregister( this );
		}

		// Required

		public abstract void OnReady();
		public abstract void OnShutdown();
	}
}
