using Espionage.Engine.Components;
using Espionage.Engine.Resources;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// An is a wrapper for a GameObject. Entities can contain I/O logic,
	/// be saved and restored, has a unique id for each, etc.
	/// </summary>
	[Group( "Entities" ), Constructor( nameof( Constructor ) ), Spawnable]
	public partial class Entity : ScriptableObject, IValid, ILibrary
	{
		public Library ClassInfo { get; private set; }

		public string Name { get; set; }
		public int Identifier { get; private set; }

		public Client Client { get; internal set; }

		// Initialization (Awake and OnDestroy, because of ScriptableObject)
		// --------------------------------------------------------------------------------------- //

		private void Awake()
		{
			ClassInfo = Library.Register( this );

			if ( ClassInfo == null )
			{
				// Something went wrong...
				Delete();
				return;
			}

			_components = new( this );
		}

		~Entity()
		{
			Debugging.Log.Info( $"Disposing {ClassInfo.Name}" );
		}

		private bool Spawned { get; set; }

		/// <summary>
		/// Spawns this Entity and places it into the game world. This most of the
		/// time will be automatically called for you. (Either by a proxy or Entity.Create)
		/// </summary>
		public void Spawn()
		{
			if ( Spawned )
			{
				Debugging.Log.Warning( "Trying to spawn, an already spawned entity." );
				return;
			}

			if ( _gameObject == null )
			{
				_gameObject = new( ClassInfo.Name );
				_gameObject.AddComponent<Hook>().Owner = this;
			}

			if ( ClassInfo.Components.Has<PersistentAttribute>() )
			{
				GameObject.DontDestroyOnLoad( _gameObject );
			}

			Identifier = GameObject.GetInstanceID();

			// Add to Database
			All.Add( this );

			OnSpawn();

			Spawned = true;
		}

		// Entity Creation + Static API
		// --------------------------------------------------------------------------------------- //

		/// <summary> All the entities that exists in the game world. </summary>
		public static Entities All { get; internal set; }

		/// <summary> Create an Entity, from its type. </summary>
		public static T Create<T>() where T : Entity, new()
		{
			return (T)Create( typeof( T ) );
		}

		/// <summary>
		/// Create an Entity, from its Library. behind the scene's it'll
		/// call a Library.Create using the fed in library and call the
		/// entities spawn method, as its implied it was called from code.
		/// If you don't it to call that, use Library.Create instead.
		/// Plus because its a library you can use the implicit operator for string
		/// to library.
		/// </summary>
		public static Entity Create( Library lib, bool spawn = true )
		{
			var ent = (Entity)Library.Create( lib );

			if ( spawn )
			{
				ent.Spawn();
			}

			return ent;
		}

		internal static Entity Constructor( Library lib )
		{
			if ( !Application.isPlaying )
			{
				Debugging.Log.Error( "Trying to create an entity, without being in Playmode" );
				return null;
			}

			var ent = (Entity)CreateInstance( lib.Info );
			return ent;
		}

		// Entity Deletion
		// --------------------------------------------------------------------------------------- //

		bool IValid.IsValid => !Deleted;

		private bool Deleted { get; set; }

		private void OnDestroy()
		{
			if ( ClassInfo == null )
			{
				// Nothing was initialized
				Debugging.Log.Error( "ClassInfo was null, when trying to destroy an entity." );
				return;
			}

			_components.Clear();
			All.Remove( this );

			Library.Unregister( this );
			OnDelete();

			Deleted = true;
			if ( _gameObject != null )
			{
				Destroy( _gameObject );
			}

			_components = null;
		}

		/// <summary>
		/// Deletes this entity by calling Scriptable.Destroy. Clean up any
		/// resource using OnDelete. Its recommended that you null all your
		/// variables in OnDelete, so the garbage collector doesnt shit itself
		/// </summary>
		public void Delete()
		{
			Destroy( this );
		}

		public static implicit operator bool( Entity exists ) => !Compare( exists, null );

		public static bool operator ==( Entity x, Entity y ) => Compare( x, y );

		public static bool operator !=( Entity x, Entity y ) => !Compare( x, y );


		private static bool Compare( Entity left, Entity right )
		{
			var leftNull = (object)left == null;
			var rightNull = (object)right == null;

			if ( rightNull & leftNull )
				return true;

			if ( rightNull )
				return !left.IsValid();

			return leftNull ? !right.IsValid() : left.Identifier == right.Identifier;
		}

		public override int GetHashCode()
		{
			return Identifier;
		}

		public override bool Equals( object other )
		{
			var rightSide = other as Entity;
			return (!(rightSide == null) || other is null or Entity) && Compare( this, rightSide );
		}

		// Frame Update
		// --------------------------------------------------------------------------------------- //

		/// <summary>
		/// Frame gets called by the entities module's frame callback.
		/// If you try deleting an entity in this call chain, it'll most likely
		/// break as the enumerable collection would of changed.
		/// </summary>
		internal void Frame()
		{
			OnFrame();
		}

		// Callbacks
		// --------------------------------------------------------------------------------------- //

		/// <inheritdoc cref="Spawn"/>
		protected virtual void OnSpawn() { }

		/// <inheritdoc cref="Frame"/>
		protected virtual void OnFrame() { }

		/// <inheritdoc cref="Delete"/>
		protected virtual void OnDelete() { }

		// Think
		// --------------------------------------------------------------------------------------- //

		/// <summary>
		/// Tick is the time it takes (in seconds), to call the active
		/// <see cref="Thinking"/> scope. Gets reset when think is called,
		/// so make sure to reset it. 
		/// </summary>
		public float Tick { set => Thinking.Tick = value; }

		/// <summary>
		/// Think gets called every tick, (which is in seconds). Use this for updating
		/// state logic on the entity in a super performant way. Since its not being called
		/// every frame, (AI, Particles, etc).
		/// </summary>
		public Thinker Thinking { get; } = new();

		// Components
		// --------------------------------------------------------------------------------------- //

		private Components<Entity> _components;

		/// <summary>
		/// The Visuals for this Entity which is the Model, Animator, etc.
		/// (This will just Get or Create the Visuals Component)
		/// </summary>
		public Visuals Visuals => Components.GetOrCreate<Visuals>();

		/// <summary>
		/// Components that are currently attached to this Entity. Use Components for
		/// injecting logic into an Entity (Dependency Injection)
		/// </summary>
		public Components<Entity> Components
		{
			get
			{
				Assert.IsInvalid( this );
				return _components;
			}
		}

		// Registering and Deserialization
		// --------------------------------------------------------------------------------------- //

		private bool _registered;

		/// <summary>
		/// This should called from an entity spawner (usually proxy), when a map is loaded.
		/// This will deserialize the data and put them into the correct properties (through reflection)
		/// override this if you want do it yourself manually (which is recommended).
		/// </summary>
		public void Register( Sheet[] values = null, Output[] outputs = null )
		{
			Assert.IsFalse( _registered = !_registered );
			OnRegister( values, outputs );
		}

		/// <inheritdoc cref="Register"/>
		protected virtual void OnRegister( Sheet[] properties, Output[] outputs )
		{
			// Use reflection to deserialize Key-Value pairs
			if ( properties != null )
			{
				Sheet.Apply( properties, this );
			}

			// Use reflection to deserialize outputs and call them
			if ( outputs == null )
			{
				return;
			}

			foreach ( var output in outputs )
			{
				var property = ClassInfo.Properties[output.Name];

				if ( property == null || property.Type != typeof( Output ) )
				{
					Debugging.Log.Warning( $"Output [{output.Name}] is not valid on [{ClassInfo.Name}]" );
					continue;
				}

				property[this] = output;
			}
		}

		// Save and Restore System
		// --------------------------------------------------------------------------------------- //

		internal void Save()
		{
			OnSave();
		}

		internal void Restore()
		{
			OnRestore();
		}

		protected virtual void OnSave() { }
		protected virtual void OnRestore() { }

		// Implicit Operators
		// --------------------------------------------------------------------------------------- //

		/// <summary>
		/// This is used for interfaces. Checks if the entity is T, if not checks the components
		/// and returns one of those. This is great if you use interfaces both in 
		/// components and entities (Such as IUse, IDamageable or IHealable)
		/// </summary>
		public T Get<T>() where T : class
		{
			Assert.IsInvalid( this );

			if ( this is T )
			{
				// It works, dont complain
				return this as T;
			}

			return Components.Get<T>();
		}

		public static implicit operator Transform( Entity entity )
		{
			return (entity ? entity : null)?.Transform;
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

		// Unity Hooks
		// --------------------------------------------------------------------------------------- //

		private GameObject _gameObject;

		public GameObject GameObject
		{
			get
			{
				Assert.IsInvalid( this );
				return _gameObject;
			}
		}

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
		public bool Enabled
		{
			// I hate Unity, this is so stupid
			get
			{
				Assert.IsInvalid( this );
				return GameObject.activeInHierarchy;
			}
			set
			{
				Assert.IsInvalid( this );
				GameObject.SetActive( value );
			}
		}

		/// <summary>
		/// What layer is this Entity in?
		/// Layers are used for rendering to cull
		/// different "layers".
		/// </summary>
		public int Layer
		{
			get
			{
				Assert.IsInvalid( this );
				return GameObject.layer;
			}
			set
			{
				Assert.IsInvalid( this );
				GameObject.layer = value;
			}
		}

		//
		// Callbacks
		//

		// Collision
		protected virtual void OnCollisionEnter( Collision collision ) { }
		protected virtual void OnCollisionExit( Collision other ) { }
		protected virtual void OnCollisionStay( Collision collisionInfo ) { }

		// Trigger
		protected virtual void OnTriggerEnter( Collider other ) { }
		protected virtual void OnTriggerExit( Collider other ) { }
		protected virtual void OnTriggerStay( Collider other ) { }
	}
}
