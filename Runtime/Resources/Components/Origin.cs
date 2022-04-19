using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	public class Origin : IComponent<IResource>, IComponent<Map>
	{
		public string Name { get; set; }

		public void OnAttached( IResource item ) { }
		public void OnAttached( Map item ) { }
	}
}
