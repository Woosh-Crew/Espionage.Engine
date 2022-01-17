using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine.Entities.Nodes
{
	public abstract partial class Node : ScriptableObject, ILibrary, ICallbacks
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

		[SerializeField]
		public Graph graph;

		[SerializeField]
		public Vector2 position;

		[SerializeField]
		private PortMap ports = new();

		//
		// Helpers
		//

		/// <summary> Iterate over all ports on this node. </summary>
		public IEnumerable<Port> Ports => ports.Values;

		/// <summary> Iterate over all outputs on this node. </summary>
		public IEnumerable<Port> Outputs => Ports.Where( port => port.IsOutput );

		/// <summary> Iterate over all inputs on this node. </summary>
		public IEnumerable<Port> Inputs => Ports.Where( port => port.IsInput );

		//
		// Serialization
		//

		[Serializable]
		private class PortMap : Map<string, Port> { }
	}
}
