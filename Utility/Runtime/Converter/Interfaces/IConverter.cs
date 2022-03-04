namespace Espionage.Engine
{
	[Group( "Converters" )]
	public interface IConverter<out T> : ILibrary
	{
		T Convert( string value );
	}
}
