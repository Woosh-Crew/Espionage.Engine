using System.Collections.Generic;
using System.Linq;
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
	public abstract class Entity : Behaviour
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

		/// <summary> Components that are currently attached to this Entity </summary>
		public Components<Entity> Components { get; private set; }

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
			base.OnDestroy();

			All.Remove( this );
			Components = null;
		}

		protected override void OnDelete() { }

		//
		// Think
		//

		internal TimeSince timeSinceLastThink;
		internal float nextThink;

		/// <summary>
		/// Tick is the time it takes (in seconds), to call <see cref="Think"/>.
		/// Gets reset when think is called, so make sure to reset it. 
		/// </summary>
		public float Tick
		{
			set
			{
				timeSinceLastThink = 0;
				nextThink = value;
			}
		}

		/// <summary>
		/// Think gets called every tick, (which is in seconds). Use this for updating
		/// state logic on the entity in a super performant way. Since its not being called
		/// every frame, (AI, Particles, etc).
		/// </summary>
		/// <param name="delta"> Time taken between the last Tick </param>
		public virtual void Think( float delta )
		{
			foreach ( var component in Components.All.OfType<IThinkable>() )
			{
				component.Think( delta );
			}
		}

		//
		// Helpers
		//

		/// <summary> Is this Entity currently Enabled? </summary>
		public bool Enabled
		{
			get => gameObject.activeInHierarchy;
			set => gameObject.SetActive( value );
		}
	}
}
