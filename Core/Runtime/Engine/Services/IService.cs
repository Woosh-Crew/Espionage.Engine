namespace Espionage.Engine.Services
{
	public interface IService
	{
		void OnReady();
		void OnShutdown();
		void OnUpdate();
	}
}
