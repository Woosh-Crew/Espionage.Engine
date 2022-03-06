using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public abstract class Component<T> : Component where T : Entity
	{
		public new T Entity => base.Entity as T;
	}

	public abstract class Component : Behaviour, IComponent<Entity>
	{
		public Entity Entity { get; private set; }

		public virtual void OnAttached( Entity item )
		{
			Entity = item;
		}
	}
}
