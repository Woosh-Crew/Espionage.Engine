namespace Espionage.Engine
{
	public interface IValid
	{
		bool IsValid { get; }
	}

	public static class Validator
	{
		public static bool IsValid( this IValid item )
		{
			return item is { IsValid: true };
		}
	}
}
