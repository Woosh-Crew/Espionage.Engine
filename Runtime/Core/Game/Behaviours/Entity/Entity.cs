using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// An Entity is the Root of a MonoBehaviour tree.
	/// Entity will cache all Components that implement
	/// the <see cref="IComponent{Entity}"/> interface or
	/// inherited from <see cref="Component{T}"/>. Entities
	/// also contain IO / Actions logic.
	/// </summary>
	[DisallowMultipleComponent, Group( "Entities" ), Constructor( nameof( Constructor ) ), Spawnable]
	public abstract class Entity : Behaviour
	{
		/// <summary> All the entities that exists in the game world. </summary>
		public static List<Entity> All { get; } = new();

		// 
		// Instance
		//

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
		}

		protected override void OnAwake() { }

		protected sealed override void OnDestroy()
		{
			All.Remove( this );
			Components = null;

			base.OnDestroy();
		}

		protected override void OnDelete() { }

		//
		// Think
		//

		internal TimeSince timeSinceLastThink;
		internal float nextThink;

		public float Delay
		{
			set
			{
				timeSinceLastThink = 0;
				nextThink = value;
			}
		}

		public virtual void Think( float delta ) { }

		//
		// Helpers
		//

		/// <summary> Is this Entity currently Enabled? </summary>
		public bool Enabled
		{
			get => gameObject.activeInHierarchy;
			set => gameObject.SetActive( value );
		}

		/// <summary> Constructs the Entity, based off the Library </summary>
		public static Entity Constructor( Library library )
		{
			return new GameObject( library.Name ).AddComponent( library.Class ) as Entity;
		}


		//
		// Components
		//

		/// <summary> Components that are currently attached to this Entity </summary>
		public Components<Entity> Components { get; private set; }
	}
}
