using System;
using System.Linq;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Behaviour is a Networked Class that at its core is just a
	/// <see cref="MonoBehaviour"/> with Espionage.Engine Functionality
	/// </summary>
	[Constructor( nameof( Constructor ) )]
	public abstract class Behaviour : MonoBehaviour, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; private set; }

		// Creation

		private void Awake()
		{
			ClassInfo = Library.Database[GetType()];
			Callback.Register( this );

			OnAwake();
		}

		protected virtual void OnAwake() { }

		// Destroy

		private void OnDestroy()
		{
			Callback.Unregister( this );
			OnDelete();
		}

		protected virtual void OnDelete() { }

		// Simulate

		public virtual void Simulate( Client client ) { }

		// Constructor

		public static object Constructor( Library library )
		{
			return new GameObject( library.Name ).AddComponent( library.Class );
		}
	}
}
