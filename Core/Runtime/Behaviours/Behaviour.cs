using System;
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

		protected virtual void Awake()
		{
			ClassInfo = Library.Database[GetType()];
			Callback.Register( this );
		}

		protected virtual void Start() { }
		protected virtual void OnEnable() { }
		protected virtual void OnDisable() { }

		protected virtual void OnDestroy()
		{
			Callback.Unregister( this );
		}

		// Library Constructor

		private static object Constructor( Library library )
		{
			return new GameObject( library.Name ).AddComponent( library.Class );
		}
	}
}
