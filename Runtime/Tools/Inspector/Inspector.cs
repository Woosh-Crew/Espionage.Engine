using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Title( "Inspector" )]
	public partial class Inspector : Window
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
					DrawGUI( Service.Selection );
					break;
				case Mode.Raw :
					RawGUI( Service.Selection );
					break;
			}

		}

		private Mode State { get; set; }

		private enum Mode { Default, Raw }

		public void SelectionChanged( ILibrary selection ) { }

		private void HeaderGUI()
		{
			ImGui.Text( $"Viewing {Service.Selection}" );
		}

		private void DrawGUI( ILibrary item )
		{
			if ( Editors.ContainsKey( item.GetType() ) )
			{
				if ( Editors[item.GetType()] != null )
				{
					ImGui.BeginGroup();

					ImGui.PushID( item.ClassInfo.Name );
					Editors[item.GetType()].OnLayout( item );
					ImGui.PopID();

					ImGui.EndGroup();
				}
				else
				{
					RawGUI( item );
				}

				return;
			}

			// Get Editor, if we haven't already
			Editors.Add( item.GetType(), GrabEditor( item is Library ? typeof( Library ) : item.ClassInfo.Info ) );
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

					foreach ( var property in item.ClassInfo.Properties.All )
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

					foreach ( var function in item.ClassInfo.Functions.All )
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

				return type == comp.Type;
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

			return lib == null ? null : Library.Database.Create<Drawer>( lib.Info );
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

			public abstract void OnLayout( Property property, ILibrary instance );
		}

		[Singleton, Group( "Inspector" )]
		public abstract class Editor : ILibrary
		{
			public Library ClassInfo { get; }

			public Editor()
			{
				ClassInfo = Library.Register( this );
			}

			public abstract void OnLayout( ILibrary item );
		}
	}
}
