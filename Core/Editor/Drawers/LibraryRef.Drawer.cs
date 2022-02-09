using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Espionage.Engine.Editor.Drawers
{
	[CustomPropertyDrawer( typeof( LibraryRef ) )]
	public class LibraryRefDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			EditorGUI.BeginProperty( position, label, property );

			// Actual GUI
			position = EditorGUI.PrefixLabel( position, label );

			var style = new GUIStyle( EditorStyles.popup ) { fixedHeight = 0 };
			if ( GUI.Button( position, new GUIContent( property.FindPropertyRelative("identifier").stringValue ), style ) )
			{
				var dropdown = new Dropdown( new AdvancedDropdownState(), this );
				dropdown.Show( position );
			}

			EditorGUI.EndProperty();
		}

		public sealed class Dropdown : AdvancedDropdown
		{
			private LibraryRefDrawer _drawer;

			public Dropdown( AdvancedDropdownState state, LibraryRefDrawer drawer ) : base( state )
			{
				_drawer = drawer;
				this.minimumSize = new Vector2( 500, 0 );
			}

			protected override void ItemSelected( AdvancedDropdownItem item ) { }

			protected override AdvancedDropdownItem BuildRoot()
			{
				var root = new AdvancedDropdownItem( "Library Database" );

				foreach ( var library in Library.Database.All.Where( e=>e.Name != e.Class.FullName ) )
				{
					root.AddChild( new AdvancedDropdownItem( library.Name ) );
				}
	
				return root;
			}
		}
	}
}
