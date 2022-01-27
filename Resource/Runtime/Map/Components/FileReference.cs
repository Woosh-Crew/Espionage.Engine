namespace Espionage.Engine.Resources
{
	public class FileReference : Map.IComponent
	{
		public string Path { get; }

		public FileReference( string path )
		{
			Path = path;
		}
		
		public void OnAttached( ref Map map ) { }
	}
}
