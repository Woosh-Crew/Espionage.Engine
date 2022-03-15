using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	public class Origin : IComponent<IResource>
	{
		public string Name { get; set; }

		public void OnAttached( IResource item ) { }
	}
}
