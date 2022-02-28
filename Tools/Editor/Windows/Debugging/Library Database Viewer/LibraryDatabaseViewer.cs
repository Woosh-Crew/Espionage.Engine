using System.Linq;
using UnityEngine.UIElements;
using UnityEditor;
using Espionage.Engine.Editor;
using UnityEngine;


namespace Espionage.Engine.Tools.Editor
{
	[Title( "Library Database" ), Group( "Debug" ), Icon( EditorIcons.Terminal )]
	public class LibraryDatabaseViewer : EditorTool
	{
		[MenuItem( "Tools/Espionage.Engine/Debug/Library Database", false, 500 )]
		private static void ShowEditor()
		{
			GetWindow<LibraryDatabaseViewer>();
		}

		private Library Active { get; set; }


		protected override void OnCreateGUI()
		{
			var all = Library.Database.All.ToList();
			var tree = new ListView( all, 20, () => new Label(), ( e, i ) => ((Label)e).text = all[i].Title ) { selectionType = SelectionType.Single };

			tree.showFoldoutHeader = true;
			tree.showBoundCollectionSize = false;
			tree.headerTitle = "fuck";

			tree.onSelectionChange += objects =>
			{
				Active = objects.First() as Library;
				Refresh();
			};

			var splitView = new TwoPaneSplitView( 0, 300, TwoPaneSplitViewOrientation.Horizontal );
			splitView.Add( tree );

			_viewerPanel = new VisualElement();
			splitView.Add( _viewerPanel );

			rootVisualElement.Add( splitView );
		}

		private VisualElement _viewerPanel;

		public void Refresh()
		{
			_viewerPanel.Clear();
			_viewerPanel.Add( BuildFromLibrary( Active ) );
		}

		protected VisualElement BuildFromLibrary( Library library )
		{
			var root = new VisualElement();

			root.Add( new HeaderBar( $"{library.Title}", "Viewing Library", null, "Header-Bottom-Border" ) );

			return root;
		}
	}
}
