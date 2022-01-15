// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor.Experimental.GraphView;
// using UnityEngine;
// using Espionage.Engine.Entities;

// using Graph = UnityEditor.Experimental.GraphView;
// using Node = Espionage.Engine.Entities.Node;

// namespace Espionage.Engine.Editor.Internal.Blueprints
// {
// 	public class BlueprintNodeUI : Graph.Node
// 	{
// 		public Node Owner => _node;
// 		private Node _node;

// 		public BlueprintNodeUI( Node node )
// 		{
// 			var info = node.ClassInfo;

// 			_node = node;
// 			title = info.Title;
// 			viewDataKey = node.id;
// 			tooltip = info.Help;

// 			style.left = node.position.x;
// 			style.top = node.position.y;

// 			CreateInputPorts();
// 		}

// 		public override void OnSelected()
// 		{
// 			base.OnSelected();

// 			Debug.Log( parent );
// 		}

// 		public override void SetPosition( Rect newPos )
// 		{
// 			base.SetPosition( newPos );

// 			_node.position.x = newPos.xMin;
// 			_node.position.y = newPos.yMin;
// 		}

// 		private void CreateInputPorts()
// 		{
// 			outputContainer.Add( InstantiatePort( Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof( Node ) ) );
// 		}


// 		private void CreateOutputPorts()
// 		{

// 		}
// 	}
// }
