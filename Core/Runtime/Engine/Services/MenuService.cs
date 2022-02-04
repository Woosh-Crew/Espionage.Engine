namespace Espionage.Engine.Services
{
	internal class MenuService : IService
	{
		private Menu Menu => Engine.Game.Menu;

		public void OnReady() { }
		public void OnShutdown() { }
		public void OnUpdate() { }
	}
}
