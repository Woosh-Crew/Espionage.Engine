namespace Espionage.Engine.Services
{
	internal class SplashService : Service
	{
		private Splash Splash => Engine.Game.Splash;

		private TimeSince _splashTime;
		private bool _finished;

		public override void OnUpdate()
		{
			if ( _splashTime >= (Splash?.Delay ?? 2) && !_finished )
			{
				_finished = true;
				Debugging.Log.Info( "Splash Loaded" );
			}
		}
	}
}
