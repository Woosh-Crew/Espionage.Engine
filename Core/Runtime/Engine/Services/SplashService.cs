namespace Espionage.Engine.Services
{
	internal class SplashService : IService
	{
		public Library ClassInfo { get; }

		public SplashService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		private TimeSince _splashTime;
		private Splash Splash => Engine.Game.Splash;

		public void OnReady() { }
		public void OnShutdown() { }

		public void OnUpdate()
		{
			// if ( _splashTime >= (Splash?.Delay ?? 2) )
			{
				// Debugging.Log.Info( "Splash Loaded" );
				// Engine.Services.Remove( this );
			}
		}
	}
}
