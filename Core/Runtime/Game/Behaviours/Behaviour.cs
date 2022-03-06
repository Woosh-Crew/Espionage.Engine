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
	[Group( "Behaviours" ), Constructor( nameof( Constructor ) )]
	public abstract class Behaviour : MonoBehaviour, ILibrary
	{
		public Library ClassInfo { get; private set; }

		// Creation

		protected virtual void Awake()
		{
			ClassInfo = Library.Register( this );
			OnAwake();
		}

		protected virtual void OnAwake() { }

		// Destroy

		protected virtual void OnDestroy()
		{
			Library.Unregister( this );
			OnDelete();
		}

		protected virtual void OnDelete() { }

		// Constructor

		public static object Constructor( Library library )
		{
			return new GameObject( library.Name ).AddComponent( library.Class );
		}
	}
}
