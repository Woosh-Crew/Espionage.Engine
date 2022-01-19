using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Espionage.Engine.Nodes.Editor
{
	public class GraphUI : GraphView
	{
		private Graph _target;

		public GraphUI()
		{
			Insert( 0, new GridBackground() );

			this.AddManipulator( new ContentZoomer() );
			this.AddManipulator( new ContentDragger() );
			this.AddManipulator( new SelectionDragger() );
			this.AddManipulator( new RectangleSelector() );
		}

		public void Populate( Graph graph )
		{
			_target = graph;

			graphViewChanged -= OnGraphChanged;
			DeleteElements( graphElements );
			graphViewChanged += OnGraphChanged;

			foreach ( var node in graph.All )
			{
				AddElement( new NodeUI( node ) );
			}
		}

		private GraphViewChange OnGraphChanged( GraphViewChange graphViewChange )
		{
			// Remove Nodes
			if ( graphViewChange.elementsToRemove is not null )
			{
				foreach ( var item in graphViewChange.elementsToRemove )
				{
					Object.Destroy( (item as NodeUI)?.Owner );
				}
			}

			// Connections to Create
			if ( graphViewChange.edgesToCreate is not null )
			{
				foreach ( var item in graphViewChange.edgesToCreate ) { }
			}

			return graphViewChange;
		}
	}
}
