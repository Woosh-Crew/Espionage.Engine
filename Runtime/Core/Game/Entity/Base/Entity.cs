using System;
using System.Linq;
using Espionage.Engine.Components;
using Espionage.Engine.Resources;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// An Entity is the Root of a MonoBehaviour tree. Entities can contain I/O logic,
	/// be saved and restored, has a unique id for each, etc.
	/// </summary>
	[DisallowMultipleComponent, Group( "Entities" ), Constructor( nameof( Constructor ) ), Spawnable, SelectionBase]
	public abstract class Entity : Behaviour, ISerializationCallbackReceiver
	{
		/// <summary> All the entities that exists in the game world. </summary>
		public static Entities All { get; } = new();

		/// <summary>
		/// Constructs the Entity, based off the Library. (Used by the
		/// [Constructor] Attribute in the Library system)
		/// </summary>
		internal static Entity Constructor( Library library )
		{
			var ent = (Entity)new GameObject( library.Name ).AddComponent( library.Info );

			ent.IsFromMap = false;
			ent.UniqueID = Guid.NewGuid();

			return ent;
		}

		/// <summary> Create an Entity, from its type. </summary>
		public static T Create<T>() where T : Entity, new()
		{
			return Library.Database.Create<T>();
		}

		/// <summary>
		/// Create an Entity, from its Library, behind the scene's it'll
		/// call a Library.Create using the fed in library. Plus because
		/// its a library you can use the implicit operator for string
		/// to library
		/// </summary>
		public static Entity Create( Library lib )
		{
			return Library.Create( lib ) as Entity;
		}

		// 
		// Instance
		//

		/// <summary>
		/// Used for grabbing entities by their Guid, such as when we
		/// are trying to invoke an output, we will use its id, so multiple
		/// can be triggered at the same time.
		/// </summary>
		public string Identifier
		{
			get => identifier;
			set => identifier = value;
		}

		public Guid UniqueID { get; private set; }
		public bool IsFromMap { get; private set; } = true;

		/// <summary> The client that has authority over this Entity </summary>
		public Client Client { get; internal set; }

		internal sealed override void Awake()
		{
			ClassInfo ??= Library.Register( this );
			All.Add( this );
			Components = new( this );

			if ( ClassInfo.Components.Has<PersistentAttribute>() )
			{
				DontDestroyOnLoad( gameObject );
			}

			OnAwake();
		}

		internal override void Start()
		{
			// Cache Components that are MonoBehaviour
			foreach ( var item in GetComponents<IComponent<Entity>>() )
			{
				Components.Add( item );
			}
		}

		protected override void OnAwake() { }

		protected sealed override void OnDestroy()
		{
			All.Remove( this );

			base.OnDestroy();

			Components.Clear();
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
		// Save & Restore
		//

		public virtual void Save( Save saver ) { }

		public virtual void Restore( Restore restore ) { }

		//
		// Helpers
		//

		public static implicit operator Transform( Entity entity )
		{
			if ( entity != null )
			{
				return entity.gameObject.transform;
			}

			Debugging.Log.Warning( "Entity was NULL" );
			return null;

		}

		public static implicit operator Entity( Guid guid )
		{
			// Find an Entity with the same GUID
			var entity = All.FirstOrDefault( e => e.UniqueID == guid );

			if ( entity != null )
			{
				return entity;
			}

			Debugging.Log.Warning( "Entity was NULL" );
			return null;

		}

		public static implicit operator GameObject( Entity entity )
		{
			if ( entity != null )
			{
				return entity.gameObject;
			}

			Debugging.Log.Warning( "Entity was NULL" );
			return null;

		}

		/// <summary>
		/// The Position of this Entity. (Feeds
		/// the value to the transforms position)
		/// </summary>
		[Serialize, Group( "Transform" ), Order( -15 )]
		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value;
		}

		/// <summary>
		/// The Rotation of this Entity.
		/// (Feeds the value to the transforms
		/// rotation)
		/// </summary>
		[Serialize, Group( "Transform" )]
		public Quaternion Rotation
		{
			get => transform.rotation;
			set => transform.rotation = value;
		}

		/// <summary>
		/// The Local Scale of this Entity.
		/// (Feeds the value to the transforms
		/// local scale)
		/// </summary>
		[Serialize, Group( "Transform" )]
		public Vector3 Scale
		{
			get => transform.lossyScale;
			set => transform.localScale = value;
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
			get => gameObject.activeInHierarchy;
			set => gameObject.SetActive( value );
		}

		// Fields & Unity Serialization

		public void OnBeforeSerialize()
		{
			if ( string.IsNullOrWhiteSpace( uniqueId ) )
			{
				uniqueId = Guid.NewGuid().ToString();
			}
		}

		public void OnAfterDeserialize()
		{
			UniqueID = Guid.Parse( uniqueId );
		}

		[SerializeField]
		private string identifier;

		[SerializeField]
		private string uniqueId;
	}
}
