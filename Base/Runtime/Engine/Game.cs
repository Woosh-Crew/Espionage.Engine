using Steamworks;

namespace Espionage.Engine
{
	public abstract class Game : ILibrary, ICallbacks
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
	}
}
