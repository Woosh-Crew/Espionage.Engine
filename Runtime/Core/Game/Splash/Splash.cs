namespace Espionage.Engine
{
	/// <summary>
	/// The splash screen hides asset and engine initialization.
	/// It is only needed on Game start. inherit from this to expand
	/// the logic, such as fading, loading background scenes for your
	/// main menu, etc.
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public class Splash : ILibrary
	{
		public Library ClassInfo { get; }

		public Splash( string splashScene, int time )
		{
			ClassInfo = Library.Database.Get( GetType() );

			Delay = time;
			Scene = splashScene;
		}

		public int Delay { get; }
		public string Scene { get; }

		public void Start()
		{
			OnStart();
		}

		protected virtual void OnStart() { }

		public void Finish()
		{
			OnFinish();
		}

		protected virtual void OnFinish() { }
	}
}
