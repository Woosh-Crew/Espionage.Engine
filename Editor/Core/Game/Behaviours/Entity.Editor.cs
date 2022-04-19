using System.Linq;
using Espionage.Engine.Internal;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Proxy ), true )]
	public class ProxyEditor : BehaviourEditor
	{
		private static LibraryList Dropdown { get; set; }

		// Instance

		private Proxy Proxy => target as Proxy;

		protected override void OnEnable()
		{
			Dropdown = new( serializedObject.FindProperty( "className" ) );

			ClassInfo = Library.Database[target.GetType()];
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title}";

			// Only Entities can have custom icons...
			if ( ClassInfo.Components.TryGet<IconAttribute>( out var icon ) )
			{
				EditorGUIUtility.SetIconForObject( target, icon.Icon );
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUILayout.BeginVertical( EngineGUI.Styles.HeaderStyle, GUILayout.MaxHeight( 64 ), GUILayout.Height( 64 ), GUILayout.ExpandWidth( true ) );
			{
				EngineGUI.Header( Dropdown, null, serializedObject.FindProperty( "name" ), serializedObject.FindProperty( "className" ), serializedObject.FindProperty( "disabled" ) );

				if ( !Proxy.className.IsEmpty() )
				{
					EngineGUI.Line();
					PropertyGUI();
				}
			}
			GUILayout.EndVertical();

			serializedObject.ApplyModifiedProperties();
		}

		private void PropertyGUI()
		{
			foreach ( var property in Library.Database[Proxy.className].Properties )
			{
				GUILayout.Label( property.Name );
			}
		}

		private void OnSceneGUI()
		{
			var proxy = (target as Proxy);
			Handles.Label( proxy.transform.position, proxy.className );
		}

		public class LibraryList : AdvancedDropdown
		{
			public SerializedProperty Owner { get; }

			public LibraryList( SerializedProperty property ) : base( new() )
			{
				minimumSize = new Vector2( 0, 200 );
				Owner = property;
			}

			protected override void ItemSelected( AdvancedDropdownItem item )
			{
				if ( !item.enabled )
				{
					return;
				}

				Owner.stringValue = Library.Database[item.id].Name;
				Owner.serializedObject.ApplyModifiedProperties();
			}

			protected override AdvancedDropdownItem BuildRoot()
			{
				var root = new AdvancedDropdownItem( "Classes" );

				var collection = Library.Database
					.Where( e => e.Info.IsSubclassOf( typeof( Entity ) ) && !e.Info.IsAbstract )
					.OrderBy( e => e.Components.Get<OrderAttribute>()?.Order ?? 5 )
					.GroupBy( e => e.Group );

				foreach ( var item in collection )
				{
					var grouping = new AdvancedDropdownItem( item.Key );
					foreach ( var library in item )
					{
						grouping.AddChild( new( $"{library.Name}" ) { id = library.Id } );
					}

					root.AddChild( grouping );
				}

				return root;
			}
		}
	}
}
