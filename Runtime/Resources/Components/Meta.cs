using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	public class Meta : IComponent<IAsset>, IComponent<Map>
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }

		public void OnAttached( IAsset item ) { }
		public void OnAttached( Map item ) { }
	}
}
