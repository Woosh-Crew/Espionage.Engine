namespace Espionage.Engine.Services
{
	internal class SplashService : IService
	{
		private Splash Splash => Engine.Game.Splash;

		public void OnReady() { }
		public void OnShutdown() {  }
		public void OnUpdate() { }
	}
}
