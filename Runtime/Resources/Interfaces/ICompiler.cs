namespace Espionage.Engine.Resources
{
	public interface ICompiler<T>
	{
		void Compile( string asset );
	}

	public interface ITester<T>
	{
		string Test( string asset );
	}
}
