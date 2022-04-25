using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	public class Origin : IComponent<IAsset>, IComponent<Map>
	{
		public string Name { get; set; }

		public void OnAttached( IAsset item ) { }
		public void OnAttached( Map item ) { }
	}
}
