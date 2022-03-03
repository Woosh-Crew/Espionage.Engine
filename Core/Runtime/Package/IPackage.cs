namespace Espionage.Engine
{
	public interface IPackage : ILibrary
	{
		void OnReady();
		void OnShutdown();
	}
}
