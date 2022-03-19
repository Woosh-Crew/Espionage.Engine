using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// An Entity is the Root of a MonoBehaviour tree. Entity will cache all Components that implement
	/// the <see cref="IComponent{Entity}"/> interface or inherited from <see cref="Component{T}"/>. Entities
	/// also contain IO / Actions logic.
	/// </summary>
	[DisallowMultipleComponent, Group( "Entities" ), Constructor( nameof( Constructor ) ), Spawnable]
	public abstract partial class Entity : Behaviour
	{
		/// <summary> All the entities that exists in the game world. </summary>
		public static List<Entity> All { get; } = new();

		/// <summary> Constructs the Entity, based off the Library </summary>
		public static object Constructor( Library library )
		{
			return new GameObject( library.Name ).AddComponent( library.Class );
		}

		// 
		// Instance
		//

		/// <summary> The client that has authority over this Entity </summary>
		public Client Client { get; internal set; }

		internal sealed override void Awake()
		{
			ClassInfo = Library.Register( this );
			All.Add( this );

			Components = new( this );

			OnAwake();

			// Cache Components
			foreach ( var item in GetComponents<IComponent<Entity>>() )
			{
				Components.Add( item );
			}

			Components.OnAdded += OnComponentAdded;
			Components.OnRemove += OnComponentRemoved;
		}

		protected override void OnAwake() { }

		protected sealed override void OnDestroy()
		{
			All.Remove( this );

			// Sometimes its null? wtf?
			if ( Components != null )
			{
				Components.OnAdded -= OnComponentAdded;
				Components.OnRemove -= OnComponentRemoved;
			}

			base.OnDestroy();

			Components = null;
		}

		protected override void OnDelete() { }

		//
		// Think
		//

		/// <summary>
		/// Think gets called every tick, (which is in seconds). Use this for updating
		/// state logic on the entity in a super performant way. Since its not being called
		/// every frame, (AI, Particles, etc).
		/// </summary>
		public Thinker Thinking { get; } = new();

		/// <summary>
		/// Tick is the time it takes (in seconds), to call the active
		/// <see cref="Thinking"/> scope. Gets reset when think is called,
		/// so make sure to reset it. 
		/// </summary>
		public float Tick
		{
			set => Thinking.Tick = value;
		}

		//
		// Components
		//

		/// <summary> Components that are currently attached to this Entity </summary>
		public Components<Entity> Components { get; private set; }

		protected virtual void OnComponentAdded( IComponent<Entity> component ) { }
		protected virtual void OnComponentRemoved( IComponent<Entity> component ) { }

		//
		// Helpers
		//

		/// <summary> The Position of this Entity. </summary>
		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value;
		}

		/// <summary> The Rotation of this Entity. </summary>
		public Quaternion Rotation
		{
			get => transform.rotation;
			set => transform.rotation = value;
		}

		/// <summary> The Local Scale of this Entity. </summary>
		public Vector3 Scale
		{
			get => transform.lossyScale;
			set => transform.localScale = value;
		}

		/// <summary> Is this Entity currently Enabled? </summary>
		public bool Enabled
		{
			get => gameObject.activeInHierarchy;
			set => gameObject.SetActive( value );
		}

	}
}
