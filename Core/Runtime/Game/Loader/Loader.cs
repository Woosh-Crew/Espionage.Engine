namespace Espionage.Engine
{
	/// <summary>
	/// This is a class for managing the loading of Maps
	/// or loading into a network game. It'll load the
	/// target scene. From that its up to you to display
	/// loading progress and text.
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public class Loader
	{
		public Library ClassInfo { get; }

		public Loader( string menuScene )
		{
			ClassInfo = Library.Database[GetType()];
			ScenePath = menuScene;
		}

		// Scene
		public string ScenePath { get; }
	}
}
