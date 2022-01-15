using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Entities
{
	[CreateAssetMenu]
	public sealed class BehaviourTree : ScriptableObject
	{
		public IReadOnlyList<Node> Nodes => nodes;

		[SerializeField]
		private List<Node> nodes = new List<Node>();

		//
		// Event Management
		//

		public void Execute( string eventName )
		{
		}

		//
		// Node Management
		//

		public Node Create( Type type )
		{
			var node = Library.Database.Create<Node>( type );
			node.name = node.ClassInfo.Title;

			nodes.Add( node );
			node.OnAdded( this );

#if UNITY_EDITOR
			node.id = new Guid().ToString();
			AssetDatabase.AddObjectToAsset( node, this );
			AssetDatabase.SaveAssets();
#endif

			return node;
		}

		public Node Find( string name )
		{
			return null;
		}

		public void Delete( Node node )
		{
			nodes.Remove( node );

#if UNITY_EDITOR
			AssetDatabase.RemoveObjectFromAsset( node );
			AssetDatabase.SaveAssets();
#endif

			ScriptableObject.DestroyImmediate( node );
		}
	}
}
