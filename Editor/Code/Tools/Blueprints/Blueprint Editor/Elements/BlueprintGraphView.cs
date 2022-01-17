using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Espionage.Engine.Entities;

namespace Espionage.Engine.Editor.Internal.Blueprints
{
	public class BlueprintGraphView : GraphView
	{
		public Blueprint Blueprint => _owner.Blueprint;
		private readonly BlueprintEditorTool _owner;

		public BlueprintGraphView( BlueprintEditorTool owner )
		{
			SetupZoom( ContentZoomer.DefaultMinScale, 3 );

			this.AddManipulator( new ContentDragger() );
			this.AddManipulator( new SelectionDragger() );
			this.AddManipulator( new RectangleSelector() );

			// Add Grid
			var grid = new GridBackground() {name = "Grid"};
			Insert( 0, grid );

			_owner = owner;
		}
	}
}
