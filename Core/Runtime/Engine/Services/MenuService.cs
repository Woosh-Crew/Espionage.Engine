namespace Espionage.Engine.Services
{
	internal class MenuService : IService
	{
		public Library ClassInfo { get; }

		public MenuService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		private Menu Menu => Engine.Game.Menu;

		public void OnReady() { }
		public void OnShutdown() { }
		public void OnUpdate() { }
	}
}
