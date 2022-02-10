namespace Espionage.Engine.Services
{
	public interface IService : ILibrary, ICallbacks
	{
		void OnReady();
		void OnShutdown();
		void OnUpdate();
	}
}
