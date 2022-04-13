using System.Reflection;

namespace Espionage.Engine
{
	public interface IMember<out T> : IMember where T : MemberInfo
	{
		T Info { get; }
	}

	public interface IMember : IMeta
	{
		Library Owner { get; set; }
		bool IsStatic { get; }
	}

	public interface IMeta
	{
		string Name { get; }
		string Title { get; set; }
		string Group { get; set; }
		string Help { get; set; }
	}
}
