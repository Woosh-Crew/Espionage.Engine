namespace Espionage.Engine
{
	public interface IDeserializer<out T> : ILibrary
	{
		T Deserialize( byte[] item );
	}
}
