namespace Espionage.Engine
{
	[Group( "Engine" )]
	public interface IPackage : ILibrary
	{
		void OnReady();
		void OnShutdown();
	}
}
