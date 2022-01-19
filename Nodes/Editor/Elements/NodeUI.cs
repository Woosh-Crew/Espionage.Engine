using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Espionage.Engine.Nodes.Editor
{
	public class NodeUI : UnityEditor.Experimental.GraphView.Node
	{
		private readonly Node _owner;

		public NodeUI( Node node )
		{
			_owner = node;

			style.left = node.position.x;
			style.top = node.position.y;
		}

		public override void SetPosition( Rect newPos )
		{
			base.SetPosition( newPos );
			_owner.position = new Vector2( newPos.x, newPos.y );
		}
	}
}
