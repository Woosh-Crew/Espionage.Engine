namespace Espionage.Engine
{
	public interface IProject
	{
		void OnCompile();
		void OnReady();
		void OnShutdown();
	}
}
