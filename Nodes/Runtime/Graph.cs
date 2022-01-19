using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Nodes
{
	/// <summary>
	/// <para>
	/// The Graph is the holder of nodes that can be invoked at runtime or editor.
	/// </para>
	/// <para>
	/// The graph is setup in a way to be able to create any sort of node structure from.
	/// This allows us to do really cool shit like Behaviour Trees, Shader Editors, 
	/// Node based audio effects, Node base animation drivers. 
	/// </para>
	/// </summary>
	public abstract class Graph : ScriptableObject, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		public void Awake()
		{
			Callback.Register( this );
		}

		private void OnDestroy()
		{
			Callback.Unregister( this );

			// Delete all nodes
			Clear();
		}

		public virtual Graph Clone()
		{
			var graph = Instantiate( this );

			for ( var i = 0; i < nodes.Count; i++ )
			{
				if ( nodes[i] == null )
				{
					continue;
				}

				var node = Instantiate( nodes[i] ) as Node;
				node.graph = graph;
				graph.nodes[i] = node;
			}

			// Redirect all connections
			foreach ( var node in graph.nodes )
			{
				if ( node == null )
				{
					continue;
				}

				foreach ( var port in node.Ports )
				{
					// port.Redirect( nodes, graph.nodes );
				}
			}

			return graph;
		}

		//
		// Tree
		//

		public IEnumerable<Node> All => nodes;

		[SerializeField]
		private List<Node> nodes;

		public void Add( Node item )
		{
			if ( item.graph is not null )
			{
				Debugging.Log.Warning( "Node was null, cannot add" );
				return;
			}

			item.graph = this;
			nodes.Add( item );
			OnNodeAdded( item );
		}

		public bool Contains( Node item )
		{
			return nodes.Contains( item );
		}

		public void Clear()
		{
			if ( Application.isPlaying )
			{
				foreach ( var item in nodes )
				{
					Destroy( item );
				}
			}

			nodes.Clear();
		}

		//
		// Callbacks
		//

		public virtual void OnNodeAdded( Node newNode ) { }
		public virtual void OnNodeDeleted( Node newNode ) { }
	}
}
