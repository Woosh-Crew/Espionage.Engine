using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	/// <summary>
	/// Menu is practically your games Main Menu.
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public class Menu : ILibrary
	{
		public Library ClassInfo { get; }

		public Menu( string menuScene )
		{
			ClassInfo = Library.Database[GetType()];
			ScenePath = menuScene;
		}

		// Scene
		public string ScenePath { get; }
	}
}
