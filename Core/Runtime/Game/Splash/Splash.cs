using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	/// <summary>
	/// The splash screen hides asset and engine initialization.
	/// It is only needed on Game start.
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public class Splash : ILibrary
	{
		public Library ClassInfo { get; }

		public Splash( string splashScene, int time )
		{
			ClassInfo = Library.Database.Get( GetType() );

			Delay = time;
			ScenePath = splashScene;
		}

		public int Delay { get; }
		public string ScenePath { get; }
	}
}
