using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	public class Meta : IComponent<IResource>
	{
		public string Title { get; }
		public string Description { get; set; }

		public Meta( string title )
		{
			Title = title;
		}

		public void OnAttached( IResource item ) { }
	}
}
