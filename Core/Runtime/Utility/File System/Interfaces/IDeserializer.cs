namespace Espionage.Engine
{
	[Group( "Files" )]
	public interface IDeserializer<out T> : ILibrary
	{
		T Deserialize( byte[] item );
	}
}
