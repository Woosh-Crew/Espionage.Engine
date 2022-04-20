namespace Espionage.Engine.Resources
{
	public interface ICompiler<in T>
	{
		void Compile( T asset );
	}

	public interface ITester<T>
	{
		string Test( string asset );
	}
}
