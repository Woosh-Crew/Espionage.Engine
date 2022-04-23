namespace Espionage.Engine.Resources
{
	public interface IResource : ILibrary
	{
		bool Persistant { get; set; }
		int Identifier { get; set; }

		void Setup( string path );

		void Load();
		bool Unload();
	}
}
