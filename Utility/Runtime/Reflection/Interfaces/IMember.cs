namespace Espionage.Engine
{
	public interface IMember
	{
		Library Owner { get; }


		string Name { get; }
		string Title { get; }
		string Group { get; }
		string Help { get; }
	}
}
