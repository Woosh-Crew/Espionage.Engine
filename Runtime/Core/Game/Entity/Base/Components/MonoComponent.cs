using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary> <inheritdoc cref="MonoComponent"/>, but with a T constraint </summary>
	public abstract class MonoComponent<T> : MonoComponent where T : Entity
	{
		/// <summary> <inheritdoc cref="Component.Entity"/> </summary>
		[Skip] public new T Entity => base.Entity as T;

		// Attaching

		public sealed override void OnAttached( Entity item )
		{
			base.OnAttached( item );
			OnAttached( Entity );
		}

		/// <summary> <inheritdoc cref="OnAttached"/> </summary>
		protected virtual void OnAttached( T item ) { }

		// Detaching

		public sealed override void OnDetached()
		{
			base.OnDetached();
			OnDetached( Entity );
		}

		/// <summary> <inheritdoc cref="OnDetached"/> </summary>
		protected virtual void OnDetached( T item ) { }

		// Can Attach

		public sealed override bool CanAttach( Entity item )
		{
			return item is T entity && CanAttach( entity );
		}

		/// <summary> <inheritdoc cref="CanAttach"/> </summary>
		protected virtual bool CanAttach( T item ) { return true; }
	}

	/// <summary>
	/// A MonoComponent is a component that is created and added through
	/// the Unity Editor. You shouldn't be using this at all. Unless its
	/// level specific components, (e.g. Volume Components)
	/// </summary>
	[Group( "Components" )]
	public abstract class MonoComponent : Behaviour, IComponent<Entity>
	{
		/// <summary> The Entity this Component is attached too. </summary>
		public Entity Entity { get; private set; }

		/// <inheritdoc cref="Component.OnAttached"/> 
		public virtual void OnAttached( Entity item )
		{
			Entity = item;
		}

		/// <inheritdoc cref="Component.OnDetached"/> 
		public virtual void OnDetached()
		{
			Entity = null;
		}

		/// <inheritdoc cref="Component.CanAttach"/> 
		public virtual bool CanAttach( Entity item )
		{
			return true;
		}
	}
}
