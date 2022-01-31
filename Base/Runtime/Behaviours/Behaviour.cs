using System;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Behaviour is a Networked Class that at its core is just a
	/// <see cref="MonoBehaviour"/> with Espionage.Engine Functionality
	/// </summary>
	public class Behaviour : MonoBehaviour, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; private set; }

		protected virtual void Awake()
		{
			ClassInfo = Library.Database[GetType()];
			Callback.Register( this );
		}

		protected virtual void OnDestroy()
		{
			Callback.Unregister( this );
		}
	}
}
