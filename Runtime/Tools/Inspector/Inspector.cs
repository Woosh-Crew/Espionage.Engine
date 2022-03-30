using System;
using System.Collections.Generic;
using ImGuiNET;
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
				DrawUnityGUI( obj );
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
		// Unity Inspector ( UnityEngine.Object )
		//

		private void DrawUnityGUI( Object item )
		{
			DrawGUI( item );
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

		public static void PropertyGUI( Property property, ILibrary instance )
		{
			if ( Drawers.ContainsKey( property.Type ) )
			{
				if ( Drawers[property.Type] != null )
				{
					ImGui.BeginGroup();

					ImGui.PushID( property.Name );
					Drawers[property.Type].OnLayout( property, instance );
					ImGui.PopID();

					ImGui.EndGroup();

					if ( ImGui.IsItemHovered() && !string.IsNullOrWhiteSpace( property.Help ) )
					{
						ImGui.SetTooltip( property.Help );
					}
				}
				else
				{
					ImGui.Text( property[instance]?.ToString() ?? "NULL" );
				}

				return;
			}

			// Get Drawer, if we haven't already
			Drawers.Add( property.Type, GrabDrawer( property.Type ) );
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
				return Library.Database.Create<EnumDrawer>();
			}

			var lib = Library.Database.Find<Drawer>( e =>
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

				return type == comp.Type;
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
			public Library ClassInfo { get; }

			public Drawer()
			{
				ClassInfo = Library.Register( this );
			}

			public abstract void OnLayout( Property property, object instance );
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
	}
}
