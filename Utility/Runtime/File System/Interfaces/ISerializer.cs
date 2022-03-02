namespace Espionage.Engine
{
	[Group( "Files" )]
	public interface ISerializer<in T> : ILibrary
	{
		byte[] Serialize( T item );
		byte[] Serialize( T[] item );
	}
}
