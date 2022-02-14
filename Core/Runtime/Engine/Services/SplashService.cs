namespace Espionage.Engine.Services
{
	internal class SplashService : IService
	{
		public Library ClassInfo { get; }
		private Splash Splash => Engine.Game.Splash;

		public SplashService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		private TimeSince _splashTime;
		private bool _finished;

		public void OnUpdate()
		{
			if ( _splashTime >= (Splash?.Delay ?? 2) && !_finished )
			{
				_finished = true;
				Debugging.Log.Info( "Splash Loaded" );
			}
		}

		// Not Needed

		public void OnReady() { }
		public void OnShutdown() { }
		public void Dispose() { }
	}
}
