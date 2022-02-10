using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public interface IConstructor : IComponent<Library>
	{
		object Invoke();
	}
}
