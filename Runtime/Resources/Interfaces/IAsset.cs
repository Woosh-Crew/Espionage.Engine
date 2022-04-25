using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	public interface IAsset : ILibrary
	{
		Resource Resource { set; }
		void Setup( Pathing path );

		void Load();
		void Unload();

		IAsset Clone();
	}
}
