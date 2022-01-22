using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Espionage.Engine.Nodes.Editor
{
	public class NodeUI : UnityEditor.Experimental.GraphView.Node
	{
		public Node Owner { get; }

		public NodeUI( Node node )
		{
			Owner = node;

			if ( node is null )
			{
				return;
			}

			style.left = node.position.x;
			style.top = node.position.y;
		}

		public override void SetPosition( Rect newPos )
		{
			base.SetPosition( newPos );

			if ( Owner is null )
			{
				return;
			}

			Owner.position = new Vector2( newPos.x, newPos.y );
		}
	}
}
