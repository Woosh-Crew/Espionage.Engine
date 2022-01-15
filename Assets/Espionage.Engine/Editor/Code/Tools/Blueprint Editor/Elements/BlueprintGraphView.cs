using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Espionage.Engine.Entities;
using UnityEditor;

using Node = Espionage.Engine.Entities.Node;
using System;

namespace Espionage.Engine.Editor.Internal.Blueprints
{
	public class BlueprintGraphView : GraphView
	{
		public Blueprint Blueprint => _owner.Blueprint;
		private BlueprintTool _owner;

		public BlueprintGraphView( BlueprintTool owner )
		{
			SetupZoom( ContentZoomer.DefaultMinScale, 3 );

			this.AddManipulator( new ContentDragger() );
			this.AddManipulator( new SelectionDragger() );
			this.AddManipulator( new RectangleSelector() );

			// Add Grid
			var grid = new GridBackground() { name = "Grid" };
			Insert( 0, grid );

			_owner = owner;
		}

		public override void BuildContextualMenu( ContextualMenuPopulateEvent evt )
		{
			base.BuildContextualMenu( evt );

			var potentialNodes = Library.Database.GetAll<Node>();

			if ( potentialNodes is null )
			{
				Debugging.Log.Warning( "Something went wrong when generating the blueprint node graph context menu" );
				return;
			}

			foreach ( var item in potentialNodes )
			{
				evt.menu.AppendAction( $"Add/{item.Title}", e => CreateNode( item.Class ) );
			}
		}

		public void CreateNode( Type type )
		{
			if ( _owner.Blueprint.Tree is null )
				return;

			var node = _owner.Blueprint.Tree.Create( type );
			CreateNodeUI( node );
		}

		//
		// Graph Loading
		//

		public void Populate()
		{
			if ( _owner.Blueprint is null )
			{
				ClearGraph();
				return;
			}

			ClearGraph();
			RecreateGraph();
		}

		private void ClearGraph()
		{
			graphViewChanged -= OnGraphViewChanged;
			DeleteElements( graphElements );
			graphViewChanged += OnGraphViewChanged;
		}

		private void RecreateGraph()
		{
			foreach ( var item in _owner.Blueprint.Tree.FUCKING_NODES )
			{
				CreateNodeUI( item );
			}
		}

		private void CreateNodeUI( Node node )
		{
			var nodeUI = new BlueprintNodeUI( node );
			AddElement( nodeUI );
		}

		//
		// Events
		//

		private GraphViewChange OnGraphViewChanged( GraphViewChange graphViewChange )
		{
			// If we have shit to remove
			if ( graphViewChange.elementsToRemove is not null )
			{
				foreach ( var item in graphViewChange.elementsToRemove )
				{
					if ( item is BlueprintNodeUI nodeUI )
					{
						_owner.Blueprint.Tree.Delete( nodeUI.Owner );
					}
				}
			}

			return graphViewChange;
		}

	}
}
