using System.Collections.Generic;
using Espionage.Engine.Components;
using Espionage.Engine.Resources;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// An Entity is the Root of a MonoBehaviour tree. Entities can contain I/O logic,
	/// be saved and restored, has a unique id for each, etc.
	/// </summary>
	[Group( "Entities" ), Spawnable]
	public abstract partial class Entity : Common, IValid
	{
		public string Name { get; set; }
		public int Identifier { get; }

		public Client Client { get; internal set; }
		public HashSet<string> Tags { get; }

		public Entity()
		{
			// Create Hook to Unity
			GameObject = new( ClassInfo.Name );
			Identifier = GameObject.GetInstanceID();
			Tags = new();

			// Create Components architecture
			Components = new( this );

			// Add to Database
			All.Add( this );

			Spawn();
		}

		// Deletion

		bool IValid.IsValid => !Deleted && GameObject != null;
		protected bool Deleted { get; private set; }

		public sealed override void Delete()
		{
			Deleted = true;

			All.Remove( this );

			base.Delete();
			OnDelete();

			GameObject.Destroy( GameObject );

			Components.Clear();
			Components = null;
		}

		protected virtual void OnDelete() { }

		protected virtual void Spawn() { }

		//
		// Think
		//

		/// <summary>
		/// Tick is the time it takes (in seconds), to call the active
		/// <see cref="Thinking"/> scope. Gets reset when think is called,
		/// so make sure to reset it. 
		/// </summary>
		public float Tick
		{
			set => Thinking.Tick = value;
		}

		/// <summary>
		/// Think gets called every tick, (which is in seconds). Use this for updating
		/// state logic on the entity in a super performant way. Since its not being called
		/// every frame, (AI, Particles, etc).
		/// </summary>
		public Thinker Thinking { get; } = new();

		//
		// Components
		//

		/// <summary>
		/// The Visuals for this Entity which is the Model, Animator, etc.
		/// (This will just Get or Create the Visuals Component)
		/// </summary>
		public Visuals Visuals => Components.GetOrCreate<Visuals>();

		/// <summary>
		/// Components that are currently attached to this Entity. Use Components for
		/// injecting logic into an Entity (Dependency Injection)
		/// </summary>
		public Components<Entity> Components { get; private set; }

		/// <summary>
		/// This is used for interfaces. Checks if the entity is T, if not checks the components
		/// and returns one of those. This is great if you use interfaces both in 
		/// components and entities (Such as IUse, IDamageable or IHealable)
		/// </summary>
		public T Get<T>() where T : class
		{
			if ( this is T )
			{
				// It works, dont complain
				return this as T;
			}

			return Components.Get<T>();
		}

		//
		// Helpers
		//

		public static implicit operator Transform( Entity entity )
		{
			if ( entity != null )
			{
				return entity.Transform;
			}

			Debugging.Log.Warning( "Entity was NULL" );
			return null;

		}

		public static implicit operator GameObject( Entity entity )
		{
			if ( entity != null )
			{
				return entity.GameObject;
			}

			Debugging.Log.Warning( "Entity was NULL" );
			return null;

		}

		public static implicit operator Entity( GameObject gameObject )
		{
			if ( gameObject != null )
			{
				return All[gameObject.GetInstanceID()];
			}

			Debugging.Log.Warning( "GameObject was NULL" );
			return null;

		}

		//
		// Unity Hooks
		//

		public GameObject GameObject { get; }

		[Serialize, Group( "Transform" ), Order( -15 )]
		public Transform Transform
		{
			get
			{
				Assert.IsInvalid( this );
				return GameObject.transform;
			}
		}

		/// <summary>
		/// The Position of this Entity. (Feeds
		/// the value to the transforms position)
		/// </summary>
		[Serialize, Group( "Transform" )]
		public Vector3 Position
		{
			get => Transform.position;
			set => Transform.position = value;
		}

		/// <summary>
		/// The Rotation of this Entity.
		/// (Feeds the value to the transforms
		/// rotation)
		/// </summary>
		[Serialize, Group( "Transform" )]
		public Quaternion Rotation
		{
			get => Transform.rotation;
			set => Transform.rotation = value;
		}

		/// <summary>
		/// The Local Scale of this Entity.
		/// (Feeds the value to the transforms
		/// local scale)
		/// </summary>
		[Serialize, Group( "Transform" )]
		public Vector3 Scale
		{
			get => Transform.lossyScale;
			set => Transform.localScale = value;
		}

		/// <summary>
		/// Is this Entity currently Enabled?
		/// (Changes gameObject.SetActive() to
		/// the target value)
		/// </summary>
		[Serialize]
		public bool Enabled
		{
			// I hate Unity, this is so stupid
			get => GameObject.activeInHierarchy;
			set => GameObject.SetActive( value );
		}

		/// <summary>
		/// What layer is this Entity in?
		/// Layers are used for rendering to cull
		/// different "layers".
		/// </summary>
		public int Layer
		{
			get => GameObject.layer;
			set => GameObject.layer = value;
		}
	}
}
