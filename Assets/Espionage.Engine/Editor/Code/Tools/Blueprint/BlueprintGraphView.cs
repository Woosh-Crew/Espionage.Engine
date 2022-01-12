using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Internal.Editor
{
	public class BlueprintGraphView : GraphView
	{
		public BlueprintGraphView()
		{
			SetupZoom( ContentZoomer.DefaultMinScale, 3 );

			this.AddManipulator( new ContentDragger() );
			this.AddManipulator( new SelectionDragger() );
			this.AddManipulator( new RectangleSelector() );

			CreateEntryPoint();

			// Add Grid
			var grid = new GridBackground() { name = "Grid" };
			Insert( 0, grid );
		}

		private void CreateEntryPoint()
		{
			var node = new Node()
			{
				title = "Start"
			};

			node.SetPosition( new Rect( 0, 0, 100, 250 ) );

			AddElement( node );
		}
	}
}
