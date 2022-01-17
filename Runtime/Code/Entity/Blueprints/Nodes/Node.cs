using System;
using UnityEngine;

namespace Espionage.Engine.Entities.Nodes
{
	public class Node : ScriptableObject, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		public void Awake()
		{
			Callback.Register( this );
		}

		private void OnDestroy()
		{
			Callback.Unregister( this );
		}
	}
}
