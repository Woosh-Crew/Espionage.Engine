using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Entities
{
	[CreateAssetMenu( fileName = "Espionage.Engine/Tree" )]
	public class NodeTree : ScriptableObject
	{
		public IReadOnlyList<Node> Nodes => nodes;

		[SerializeField]
		private List<Node> nodes = new List<Node>();


		//
		// Node Management
		//

		public Node Create( Type type )
		{
			var newNode = Library.Database.Create( type ) as Node;
			newNode.name = newNode.ClassInfo.Title;
			newNode.id = GUID.Generate().ToString();
			nodes.Add( newNode );

#if UNITY_EDITOR
			AssetDatabase.AddObjectToAsset( newNode, this );
			AssetDatabase.SaveAssets();
#endif

			return newNode;
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
