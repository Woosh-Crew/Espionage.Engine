namespace Espionage.Engine
{
	public interface ISerializer<in T> 
	{
		byte[] Serialize( T item );
	}
}
