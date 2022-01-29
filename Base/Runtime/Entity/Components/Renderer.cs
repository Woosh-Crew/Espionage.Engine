namespace Espionage.Engine.Components
{
	/// <summary>
	/// A Simple Component that can get attached to entities
	/// to render objects. Under the hood it just spawns a GameObject
	/// with the respective Model
	/// </summary>
	public class Renderer : IComponent<Entity>
	{
		// Model
		// Transform

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
