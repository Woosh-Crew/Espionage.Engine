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

			var idProperty = property.FindPropertyRelative( "identifier" );
			var style = new GUIStyle( EditorStyles.popup );
			if ( GUI.Button( position, new GUIContent( string.IsNullOrEmpty( idProperty.stringValue ) ? "None" : idProperty.stringValue ), style ) )
			{
				var dropdown = new Dropdown( new AdvancedDropdownState(), idProperty );
				dropdown.Show( position );
			}

			EditorGUI.EndProperty();
		}

		public sealed class Dropdown : AdvancedDropdown
		{
			private readonly SerializedProperty _property;

			public Dropdown( AdvancedDropdownState state, SerializedProperty property ) : base( state )
			{
				_property = property;
				minimumSize = new Vector2( 0, 250 );
			}

			protected override void ItemSelected( AdvancedDropdownItem item )
			{
				_property.stringValue = item.name;
				_property.serializedObject.ApplyModifiedProperties();
			}

			protected override AdvancedDropdownItem BuildRoot()
			{
				var root = new AdvancedDropdownItem( "Library Database" );

				var groups = Library.Database.All.Where( e => e.Name != e.Class.FullName ).OrderBy( e => e.Name ).GroupBy( e => e.Group );

				foreach ( var item in groups )
				{
					var itemRoot = new AdvancedDropdownItem( item.Key );
					root.AddChild( itemRoot );

					foreach ( var library in item )
					{
						itemRoot.AddChild( new AdvancedDropdownItem( library.Name ) );
					}
				}

				return root;
			}
		}
	}
}
