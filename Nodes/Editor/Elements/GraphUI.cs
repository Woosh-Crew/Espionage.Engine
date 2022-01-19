using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Espionage.Engine.Nodes.Editor
{
	public class GraphUI : GraphView
	{
		private readonly Graph _owner;

		public GraphUI( Graph graph )
		{
			_owner = graph;

			Insert( 0, new GridBackground() );

			this.AddManipulator( new ContentZoomer() );
			this.AddManipulator( new ContentDragger() );
			this.AddManipulator( new SelectionDragger() );
			this.AddManipulator( new RectangleSelector() );

			Populate( _owner );
		}

		public void Populate( Graph graph )
		{
			DeleteElements( graphElements );

			foreach ( var node in graph.All )
			{
				AddElement( new NodeUI( node ) );
			}
		}
	}
}
