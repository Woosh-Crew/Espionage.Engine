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
			var print1 = this.Create<PrintNode>();
			var print2 = this.Create<PrintNode>();
			print1.child = print2;

			var print3 = this.Create<PrintNode>();
			print2.child = print3;

			print1.Execute();
		}

		//
		// Node Management
		//

		public Node Create( Type type )
		{
			var node = Library.Database.Create<Node>( type );
			node.name = node.ClassInfo.Title;
			node._tree = this;

			nodes.Add( node );

#if UNITY_EDITOR
			node.id = new Guid().ToString();
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
