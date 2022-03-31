using System;
using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Services;
using ImGuiNET;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Tools
{
	public sealed class Inspector : Window
	{
		public override void OnLayout()
		{
			// Don't render if there is nothing there.
			if ( Service.Selection == null )
			{
				ImGui.Text( "Nothing Selected!" );
				return;
			}

			HeaderGUI();
			ImGui.Separator();

			// Library UI
			if ( Service.Selection is ILibrary lib )
			{
				// Draw UI
				if ( ImGui.BeginTabBar( "Inspector Bar" ) )
				{
					if ( ImGui.TabItemButton( "Default" ) )
					{
						State = Mode.Default;
					}

					if ( ImGui.TabItemButton( "Raw" ) )
					{
						State = Mode.Raw;
					}
				}

				ImGui.EndTabBar();

				switch ( State )
				{
					case Mode.Default :
						DrawGUI( lib );
						break;
					case Mode.Raw :
						RawGUI( lib );
						break;
				}
			}

			// Unity UI
			else if ( Service.Selection is Object obj )
			{
				// DrawUnityGUI( obj );
			}
		}

		private Mode State { get; set; }

		private enum Mode { Default, Raw }

		public void SelectionChanged( object selection )
		{
			if ( selection is ILibrary lib )
			{
				if ( !Editors.ContainsKey( selection.GetType() ) )
				{
					Editors.Add( selection.GetType(), GrabEditor( lib is Library ? typeof( Library ) : lib.ClassInfo.Info ) );
				}

				Editors[selection.GetType()]?.OnActive( lib );
			}
		}

		private void HeaderGUI()
		{
			ImGui.Text( $"Viewing {Service.Selection}" );
		}

		//
		// ILibrary Inspector
		//

		private void DrawGUI( object item )
		{
			if ( Editors.ContainsKey( item.GetType() ) )
			{
				if ( Editors[item.GetType()] != null )
				{
					ImGui.BeginGroup();

					ImGui.PushID( item.ToString() );
					Editors[item.GetType()].OnLayout( item );
					ImGui.PopID();

					ImGui.EndGroup();
				}
				else
				{
					if ( item is ILibrary library )
					{
						RawGUI( library );
					}
				}

				return;
			}

			// Get Editor, if we haven't already
			Editors.Add( item.GetType(), GrabEditor( item is Library ? typeof( Library ) : item.GetType() ) );
		}

		private void RawGUI( ILibrary item )
		{
			if ( item.ClassInfo == null )
			{
				ImGui.TextColored( Color.red, "NULL ClassInfo" );
				return;
			}

			// Us doing this removes the title.. but we gotta or else the scrolling just doesnt work
			if ( ImGui.BeginChild( "out", new( 0, ImGui.GetWindowHeight() - 96 ), false ) )
			{
				ImGui.Text( "Properties" );

				// Properties
				if ( ImGui.BeginTable( "Properties", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
				{
					ImGui.TableSetupColumn( "Name", ImGuiTableColumnFlags.WidthFixed, 96 );
					ImGui.TableSetupColumn( "Value" );

					ImGui.TableHeadersRow();

					foreach ( var property in item.ClassInfo.Properties )
					{
						ImGui.TableNextColumn();
						ImGui.Text( property.Title );

						ImGui.TableNextColumn();
						ImGui.SetNextItemWidth( ImGui.GetColumnWidth( 1 ) );
						PropertyGUI( property, item );
					}
				}

				ImGui.EndTable();

				ImGui.Separator();

				ImGui.Text( "Functions" );

				// Functions
				if ( ImGui.BeginTable( "Functions", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
				{
					ImGui.TableSetupColumn( "Name", ImGuiTableColumnFlags.WidthFixed, 96 );
					ImGui.TableSetupColumn( "Invoke" );

					ImGui.TableHeadersRow();

					foreach ( var function in item.ClassInfo.Functions )
					{
						ImGui.TableNextColumn();
						ImGui.Text( function.Title );

						ImGui.TableNextColumn();
						ImGui.SetNextItemWidth( ImGui.GetColumnWidth( 1 ) );
						if ( ImGui.Selectable( "Invoke" ) )
						{
							function.Invoke( item );
						}
					}
				}

				ImGui.EndTable();
			}

			ImGui.EndChild();
		}

		public static void PropertyGUI( Property property, object instance )
		{
			if ( PropertyGUI( property.Type, property, instance, property[instance], out var changed ) )
			{
				property[instance] = changed;
			}
		}

		public static bool PropertyGUI( Type target, Property property, object instance, object value, out object changed )
		{
			if ( !Drawers.ContainsKey( target ) )
			{
				Drawers.Add( target, GrabDrawer( target ) );

				changed = null;
				return false;
			}

			if ( Drawers[target] == null )
			{
				ImGui.TextDisabled( value?.ToString() ?? "Null" );
				if ( ImGui.IsItemHovered() )
				{
					ImGui.SetTooltip( "Item is not inherited from ILibrary, or there is no valid drawer" );
				}

				changed = null;
				return false;
			}

			// Normal Drawer
			ImGui.BeginGroup();
			{
				ImGui.PushID( property?.Name ?? target.Name );

				var drawer = Drawers[target];
				drawer.Type = target;
				drawer.Property = property;

				drawer.OnLayout( instance, value, out changed );

				ImGui.PopID();
			}
			ImGui.EndGroup();

			if ( ImGui.IsItemHovered() && !string.IsNullOrWhiteSpace( property?.Help ) )
			{
				ImGui.SetTooltip( property.Help );
			}

			return changed != default;
		}

		private static Editor GrabEditor( Type type )
		{
			var lib = Library.Database.Find<Editor>( e =>
			{
				var comp = e.Components.Get<TargetAttribute>();

				if ( comp == null )
				{
					return false;
				}

				if ( comp.Type.IsInterface )
				{
					// Generic interface
					return type.HasInterface( comp.Type );
				}

				return type == comp.Type || type.IsSubclassOf( comp.Type );
			} );

			return lib == null ? null : Library.Database.Create<Editor>( lib.Info );
		}

		private static Drawer GrabDrawer( Type type )
		{
			if ( type.IsEnum )
			{
				// Explicit Enum Drawer
				return Library.Database.Create<EnumDrawer>();
			}

			if ( type.IsArray )
			{
				// Explicit Array Drawer
				return Library.Database.Create<ArrayDrawer>();
			}

			// See if we can use Type Bound.
			var lib = Library.Database.Find<Drawer>( e => type == e.Components.Get<TargetAttribute>()?.Type );

			// Now see if we can use interface bound if its null.
			lib ??= Library.Database.Find<Drawer>( e =>
			{
				var comp = e.Components.Get<TargetAttribute>();

				if ( comp == null )
				{
					return false;
				}

				return comp.Type.IsInterface && type.HasInterface( comp.Type );
			} );

			if ( lib == null )
			{
				// See if we have one from ILibrary
				return type.HasInterface<ILibrary>() ? Library.Database.Create<ILibraryDrawer>() : null;
			}

			return Library.Database.Create<Drawer>( lib.Info );
		}

		//
		// UI
		// 

		private static readonly Dictionary<Type, Drawer> Drawers = new();
		private static readonly Dictionary<Type, Editor> Editors = new();

		[Singleton, Group( "Inspector" )]
		public abstract class Drawer : ILibrary
		{
			// Data

			public Type Type { get; internal set; }
			public Property Property { get; internal set; }

			// Instance

			public Library ClassInfo { get; }

			public Drawer()
			{
				ClassInfo = Library.Register( this );
			}

			/// <param name="property"> [Nullable] the property that owns this drawer</param>
			/// <param name="instance"> The current instance we're iterating through </param>
			/// <param name="value"> The current value of this property </param>
			/// <param name="change"> The value we should change to if this returns true. </param>
			/// <returns>True if the value of this property has changed</returns>
			public abstract bool OnLayout( object instance, in object value, out object change );
		}

		public abstract class Drawer<T> : Drawer
		{
			public override bool OnLayout( object instance, in object value, out object change )
			{
				// String has its own NULL stuff. This is hacky, but can't do much about that.
				if ( typeof( T ) != typeof( string ) && value == null )
				{
					ImGui.Text( "Null" );

					change = null;
					return false;
				}

				try
				{
					if ( OnLayout( instance, (T)value, out var changed ) )
					{
						change = changed;
						return true;
					}
				}
				catch ( InvalidCastException )
				{
					Dev.Log.Info( $"Tried casting {value.GetType()} to {typeof( T ).Name} " );
				}


				change = null;
				return false;
			}

			/// <inheritdoc cref="Drawer.OnLayout"/>
			protected abstract bool OnLayout( object instance, in T value, out T change );
		}

		[Singleton, Group( "Inspector" )]
		public abstract class Editor : ILibrary
		{
			public Library ClassInfo { get; }

			public Editor()
			{
				ClassInfo = Library.Register( this );
			}

			public virtual void OnActive( object item ) { }
			public abstract void OnLayout( object instance );
		}

		public abstract class Editor<T> : Editor
		{
			public override void OnActive( object item )
			{
				OnActive( (T)item );
			}

			public override void OnLayout( object instance )
			{
				OnLayout( (T)instance );
			}

			protected virtual void OnActive( T item ) { }
			protected abstract void OnLayout( T instance );
		}
	}

}
