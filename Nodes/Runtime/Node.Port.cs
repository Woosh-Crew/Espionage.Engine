using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine.Nodes
{
	public abstract partial class Node
	{
		/// <summary>
		/// Used on properties to define Inputs.
		/// </summary>
		[AttributeUsage( AttributeTargets.Property, Inherited = true )]
		protected class InputAttribute : Attribute { }

		/// <summary>
		/// Used on properties to define Outputs.
		/// </summary>
		[AttributeUsage( AttributeTargets.Property, Inherited = true )]
		protected class OutputAttribute : Attribute { }

		[Serializable]
		public sealed class Port
		{
			public enum IO { Input, Output }

			public enum Capacity { Single, Multiple }

			public Port()
			{
				connections = new List<Connection>();
			}

			// Helpers
			public Node Node => node;
			public IO Direction => direction;
			public bool IsConnected => connections.Count > 0;
			public bool IsInput => direction is IO.Input;
			public bool IsOutput => direction is IO.Output;

			// Data
			[SerializeField]
			private string name;

			[SerializeField]
			private Node node;

			[SerializeField]
			private IO direction;

			[SerializeField]
			private List<Connection> connections;

			//
			// Connect
			//

			public bool IsConnectedTo( Port port )
			{
				return connections.Any( t => t.Port == port );
			}

			public void Connect( Port port )
			{
				if ( !CanConnect( port ) )
				{
					Debugging.Log.Warning( "Cannot Connect Port" );
					return;
				}

				Node.OnCreateConnection( this, port );
				port.Node.OnCreateConnection( this, port );
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

			//
			// Disconnect
			//

			public void Disconnect( Port port )
			{
				// Remove connections from this to port
				connections.RemoveAll( e => e.Port == port );

				// Now remove connections from port to this
				port.connections.RemoveAll( e => e.Port == this );

				// Trigger Disconnection Event
				Node.OnRemoveConnection( this );
				if ( port.IsConnectedTo( this ) )
				{
					port.node.OnRemoveConnection( port );
				}
			}

			public void Clear()
			{
				foreach ( var connection in connections )
				{
					Disconnect( connection.Port );
				}
			}

			//
			// Serialization
			//

			[Serializable]
			private class Connection
			{
				public string propertyName;
				public Node node;

				public Port Port => _cachedPort ?? FindPort();
				private Port _cachedPort;

				public List<Vector2> reroutes;

				public Connection( Port target )
				{
					node = target.node;
					propertyName = target.name;
				}

				private Port FindPort()
				{
					if ( node is null || string.IsNullOrEmpty( propertyName ) )
					{
						return null;
					}

					_cachedPort = node.GetPort( propertyName );
					return _cachedPort;
				}
			}
		}
	}
}
