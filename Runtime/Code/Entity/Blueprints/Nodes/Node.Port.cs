using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities.Nodes
{
	public abstract partial class Node
	{
		[Serializable]
		public sealed class Port
		{
			public enum IO { Input, Output }

			// Helpers
			public Node Node => _node;
			public IO Direction => _direction;
			public bool IsInput => _direction is IO.Input;
			public bool IsOutput => _direction is IO.Output;

			// Data
			[SerializeField]
			private Node _node;
			
			[SerializeField]
			private IO _direction;

			public void Connect( Port port )
			{
				if ( !CanConnect( port ) )
				{
					Debugging.Log.Warning( "Cannot Connect Port" );
					return;
				}
			}

			public bool CanConnect( Port port )
			{
				if ( port is null )
				{
					return false;
				}

				if ( port.Direction == Direction )
				{
					return false;
				}

				return true;
			}
		
			[Serializable]
			private class Connection
			{
				public Node node;
				public Port Port => _port;

				[SerializeField] private Port _port;
				 public List<Vector2> reroutePoints = new List<Vector2>();

				public Connection( Port port ) {
					this._port = port;
					node = port.Node;
				}
			}
		}
	}
}
