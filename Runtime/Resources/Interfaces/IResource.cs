namespace Espionage.Engine.Resources
{
	public interface IResource : ILibrary
	{
		int Identifier { get; set; }
		bool Persistant { get; set; }
		
		void Setup( string path );
		
		void Load();
		bool Unload();
	}
}
