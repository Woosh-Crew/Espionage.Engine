using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Espionage.Engine.Nodes.Editor
{
	public class GraphUI : GraphView
	{
		public GraphUI()
		{
			Insert(0, new GridBackground());
			
			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
		}
	}
}
