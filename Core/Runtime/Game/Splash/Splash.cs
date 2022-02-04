namespace Espionage.Engine
{
	/// <summary>
	/// The splash screen hides asset and engine initialization.
	/// It is only needed on Game start.
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public class Splash : ILibrary, ICallbacks
	{
		public Library ClassInfo { get; }

		public Splash( string splashScene )
		{
			ClassInfo = Library.Database.Get( GetType() );
			Callback.Register( this );

			ScenePath = splashScene;
		}

		~Splash()
		{
			Callback.Unregister( this );
		}

		// Scene
		public string ScenePath { get; }
	}
}
