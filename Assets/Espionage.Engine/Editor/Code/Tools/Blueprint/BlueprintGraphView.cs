using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Espionage.Engine.Entities;
using UnityEditor;

namespace Espionage.Engine.Internal.Editor
{
	public class BlueprintGraphView : GraphView
	{
		private BehaviourTree _tree;

		public BlueprintGraphView()
		{
			SetupZoom( ContentZoomer.DefaultMinScale, 3 );

			this.AddManipulator( new ContentDragger() );
			this.AddManipulator( new SelectionDragger() );
			this.AddManipulator( new RectangleSelector() );

			var eventsBlackboard = new Blackboard( this ) { title = "Variables" };

			Add( eventsBlackboard );
			Add( new Blackboard( this ) { title = "Events" } );


			// Add Grid
			var grid = new GridBackground() { name = "Grid" };
			Insert( 0, grid );

		}

		internal void Populate( BehaviourTree tree )
		{
			_tree = tree;
		}
	}
}
