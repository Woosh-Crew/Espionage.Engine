namespace Espionage.Engine.Mod
{
	public class Mod : ILibrary
	{
		public Library ClassInfo { get; }
		public string Name { get; }
		public string Path { get; }

		public Mod( string name, string path )
		{
			ClassInfo = Library.Register( this );
			Name = name;
			Path = path;
		}
	}
}
