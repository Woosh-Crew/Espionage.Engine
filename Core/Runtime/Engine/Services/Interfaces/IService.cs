namespace Espionage.Engine.Services
{
	public interface IService : ILibrary
	{
		void OnReady();
		void OnShutdown();
		void OnUpdate();
	}
}
