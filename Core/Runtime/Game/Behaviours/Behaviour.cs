using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Behaviour is a Networked Class that at its core is just a
	/// <see cref="MonoBehaviour"/> with Espionage.Engine Functionality
	/// </summary>
	[Group( "Behaviours" ), Spawnable( false )]
	public abstract class Behaviour : MonoBehaviour, ILibrary
	{
		public Library ClassInfo { get; internal set; }

		// Creation

		protected virtual void Awake()
		{
			ClassInfo = Library.Register( this );
			OnAwake();
		}

		protected virtual void OnAwake() { }

		// Ready

		private void Start()
		{
			OnReady();
		}

		protected virtual void OnReady() { }

		// Destroy

		protected virtual void OnDestroy()
		{
			Library.Unregister( this );
			OnDelete();
		}

		protected virtual void OnDelete() { }

	}
}
