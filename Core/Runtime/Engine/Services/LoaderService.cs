namespace Espionage.Engine.Services
{
	public class LoaderService : IService
	{
		public Library ClassInfo { get; }

		public LoaderService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		public Loader Loader => Engine.Game.Loader;

		public void OnReady() {  }
		public void OnShutdown() { }
		public void OnUpdate() {  }
	}
}
