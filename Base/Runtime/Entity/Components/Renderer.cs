namespace Espionage.Engine.Components
{
	public class Renderer : IComponent<Entity>
	{
		public void OnAttached( Entity item )
		{
			// Spawn Model GameObject from Assets
		}

		public void OnDetached( Entity item )
		{
			// Delete GameObject
		}	
	}
}
