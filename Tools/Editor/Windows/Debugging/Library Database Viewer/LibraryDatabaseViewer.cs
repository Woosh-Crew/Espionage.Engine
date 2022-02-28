using System.Linq;
using UnityEngine.UIElements;
using UnityEditor;
using Espionage.Engine.Editor;
using UnityEngine;

namespace Espionage.Engine.Tools.Editor
{
	[Title( "Library Database Viewer" ), Group( "Debug" ), Icon( EditorIcons.Terminal )]
	public class LibraryDatabaseViewer : EditorTool
	{
		// Menu Items

		[MenuItem( "Tools/Espionage.Engine/Debug/Library Database", false, 500 )]
		private static void ShowEditor()
		{
			GetWindow<LibraryDatabaseViewer>();
		}

		[Function, Menu( "Viewer/Clear" )]
		private void RemoveActive() { }

		// UI

		private Library Active { get; set; }

		protected override void OnCreateGUI()
		{
			var all = Library.Database.All.GroupBy( e => e.Group ).OrderBy( e => e.Key );

			var treeContainer = new ScrollView( ScrollViewMode.Vertical );

			var titleBar = new TitleBar( "Library Database", null, "Bottom" );
			titleBar.style.marginBottom = 8;
			treeContainer.Add( titleBar );

			foreach ( var item in all )
			{
				var database = item.ToList();

				var tree = new ListView( database, 20, () =>
				{
					var label = new Label();

					label.style.paddingLeft = 12;
					label.style.unityTextAlign = TextAnchor.MiddleLeft;

					return label;
				}, ( e, i ) => ((Label)e).text = database[i].Title )
				{
					selectionType = SelectionType.Single,
					showFoldoutHeader = true,
					showBoundCollectionSize = false,
					headerTitle = string.IsNullOrEmpty( item.Key ) ? "No Group" : item.Key
				};

				tree.onSelectionChange += objects =>
				{
					Active = objects.First() as Library;
					Refresh();
				};

				tree.Query<Foldout>().First().value = false;
				tree.style.paddingBottom = 8;
				treeContainer.Add( tree );
			}

			// Create Split View

			var splitView = new TwoPaneSplitView( 0, 300, TwoPaneSplitViewOrientation.Horizontal );
			splitView.Add( treeContainer );

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

			root.Add( new HeaderBar( $"{library.Title}", $"Viewing {library.Name}", null, "Header-Bottom-Border" ) );

			return root;
		}
	}
}
