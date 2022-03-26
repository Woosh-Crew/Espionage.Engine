namespace Espionage.Engine
{
	public interface IMember : IMeta
	{
		Library Owner { get; set; }
	}

	public interface IMeta
	{
		string Name { get; }
		string Title { get; set; }
		string Group { get; set; }
		string Help { get; set; }
	}
}
