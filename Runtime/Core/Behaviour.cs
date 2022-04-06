using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Behaviour is a Network-able Class (Planned) that at its core is just a 
	/// <see cref="MonoBehaviour"/> with Espionage.Engine Functionality, e.g,
	/// library cache and callbacks.
	/// </summary>
	[Group( "Behaviours" ), Spawnable( false )]
	public abstract class Behaviour : MonoBehaviour, ILibrary
	{
		public Library ClassInfo { get; internal set; }

		// Creation

		internal virtual void Awake()
		{
			ClassInfo ??= Library.Register( this );

			// If no Library, Something went wrong.
			// Just destroy.
			if ( ClassInfo == null )
			{
				Destroy( this );
				return;
			}

			OnAwake();
		}

		/// <summary>
		/// This Behaviour is now Awake.
		/// Use this for any initialization.
		/// </summary>
		protected virtual void OnAwake() { }

		// Ready

		internal virtual void Start() { }

		// Destroy

		public void Delete()
		{
			if ( ClassInfo != null )
			{
				Library.Unregister( this );
			}

			Destroy( this );
		}

		protected virtual void OnDestroy()
		{
			if ( ClassInfo != null )
			{
				Library.Unregister( this );
			}

			OnDelete();
		}

		/// <summary>
		/// This object is being deleted.
		/// Use this for cleaning up your
		/// behaviours variables.
		/// </summary>
		protected virtual void OnDelete() { }

	}
}
