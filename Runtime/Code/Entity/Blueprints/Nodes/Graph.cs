using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities.Nodes
{
	public sealed class Graph : ScriptableObject, ILibrary, ICallbacks
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

		// Tree
		public List<Node> nodes;
	}
}
