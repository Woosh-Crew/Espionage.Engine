using System;
using UnityEngine;

namespace Espionage.Engine
{
	public class Behaviour : MonoBehaviour, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; private set; }

		private void Awake()
		{
			ClassInfo = Library.Database[GetType()];
			Callback.Register(this);
		}

		protected virtual void OnDestroy()
		{
			Callback.Unregister( this );
		}
	}
}
