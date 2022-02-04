namespace Espionage.Engine.Services
{
	internal class SplashService : IService
	{
		public Library ClassInfo { get; }

		public SplashService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		private Splash Splash => Engine.Game.Splash;

		public void OnReady() { }
		public void OnShutdown() { }
		public void OnUpdate() { }
	}
}
