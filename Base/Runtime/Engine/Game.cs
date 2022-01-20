namespace Espionage.Engine
{
	public abstract class Game : ILibrary, ICallbacks, IProject
	{
		public Library ClassInfo { get; }

		public Game()
		{
			ClassInfo = Library.Database.Get( GetType() );
			Callback.Register( this );
		}

		~Game()
		{
			Callback.Unregister( this );
		}

		//
		// Required
		//

		public abstract void OnReady();
		public abstract void OnShutdown();
		public abstract void OnCompile();
	}
}
