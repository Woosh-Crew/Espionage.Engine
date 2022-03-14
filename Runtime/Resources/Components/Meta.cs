using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	public class Meta : IComponent<IResource>
	{
		public string Title { get; set; }
		public string Description { get; set; }

		public void OnAttached( IResource item ) { }
	}
}
