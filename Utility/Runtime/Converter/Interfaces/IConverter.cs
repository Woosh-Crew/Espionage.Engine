namespace Espionage.Engine
{
	public interface IConverter<out T> : ILibrary
	{
		T Convert( string value );
	}
}
