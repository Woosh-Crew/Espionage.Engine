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
		/// <summary>
		/// All of the Entities that currently exists in
		/// the game world. This is great for finding
		/// entities from any class. Such as spawn
		/// points, gamemodes, map meta data, etc.
		/// </summary>
		public static List<Entity> All { get; } = new();

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

		protected sealed override void OnDestroy()
		{
			All.Remove( this );
			Components = null;

			base.OnDelete();
		}

		// Constructor

		private static Entity Constructor( Library library )
		{
			return new GameObject( library.Name ).AddComponent( library.Class ) as Entity;
		}

		// Helpers

		/// <summary> Is this Entity currently Enabled? </summary>
		public bool Enabled
		{
			get => gameObject.activeInHierarchy;
			set => gameObject.SetActive( value );
		}

		// Components

		/// <summary> Components that are currently attached to this Entity </summary>
		public Components<Entity> Components { get; private set; }
	}
}
