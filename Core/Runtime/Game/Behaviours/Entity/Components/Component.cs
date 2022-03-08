using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary> <inheritdoc cref="Component"/> </summary>
	public abstract class Component<T> : Component where T : Entity
	{
		public new T Entity => base.Entity as T;

		public override bool CanAttach( Entity item )
		{
			return item is T;
		}
	}

	/// <summary>
	/// A Component is a MonoBehaviour that
	/// gets attached to an Entity
	/// </summary>
	public abstract class Component : Behaviour, IComponent<Entity>
	{
		public Entity Entity { get; private set; }

		protected override void OnReady()
		{
			if ( Entity == null )
			{
				Debugging.Log.Error( "No Entity found on component" );
			}
		}

		public virtual void OnAttached( Entity item )
		{
			Entity = item;
		}

		public virtual bool CanAttach( Entity item )
		{
			return true;
		}
	}
}
