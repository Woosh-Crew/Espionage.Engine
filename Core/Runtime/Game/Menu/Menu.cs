using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	/// <summary>
	/// Menu is practically your games Main Menu.
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public class Menu : ILibrary, ICallbacks
	{
		public Library ClassInfo { get; }

		public Menu( string menuScene )
		{
			ClassInfo = Library.Database[GetType()];
			Callback.Register( this );

			ScenePath = menuScene;
		}

		~Menu()
		{
			Callback.Unregister( this );
		}

		// Scene
		public string ScenePath { get; }
	}
}
