using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Entities
{
	public sealed class BehaviourTree : ScriptableObject
	{
		public IReadOnlyList<Node> Nodes => nodes;

		[SerializeField]
		private List<Node> nodes = new List<Node>();

		//
		// Node Management
		//

		public Node Create( Type type )
		{
			var node = Library.Database.Create<Node>( type );
			node.name = node.ClassInfo.Title;
			node.id = new Guid().ToString();
			nodes.Add( node );

#if UNITY_EDITOR
			AssetDatabase.AddObjectToAsset( node, this );
			AssetDatabase.SaveAssets();
#endif

			return node;
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
