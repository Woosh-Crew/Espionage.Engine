using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary> <inheritdoc cref="Component"/>, but with a T constraint </summary>
	public abstract class Component<T> : Component where T : Entity
	{
		/// <summary> <inheritdoc cref="Component.Entity"/> </summary>
		public new T Entity => base.Entity as T;

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
	/// A Component is a MonoBehaviour that
	/// gets attached to an Entity
	/// </summary>
	public abstract class Component : Behaviour, IComponent<Entity>
	{
		// Sealed

		internal void Ready()
		{
			OnReady();
		}

		internal override void Start()
		{
			if ( Entity == null )
			{
				Dev.Log.Error( "No Entity found on component" );
			}
		}

		protected sealed override void OnAwake()
		{
			base.OnAwake();
		}

		protected sealed override void OnDelete()
		{
			if ( Entity == null || Entity.Components == null )
			{
				OnDetached();
			}
			else
			{
				Entity.Components?.Remove( this );
			}

		}

		// Component

		/// <summary> The Entity this Component is attached too. </summary>
		public Entity Entity { get; private set; }

		/// <summary>
		/// What should we do when this component
		/// is attached to an Entity
		/// </summary>
		public virtual void OnAttached( Entity item )
		{
			Entity = item;
			ClassInfo ??= Library.Register( this );
		}

		/// <summary>
		/// What should we do when this component
		/// is detached to from an Entity.
		/// </summary>
		public virtual void OnDetached()
		{
			Entity = null;
		}

		/// <summary>
		/// Can this component be attached
		/// to this Entity?
		/// </summary>
		public virtual bool CanAttach( Entity item )
		{
			return true;
		}
	}
}
