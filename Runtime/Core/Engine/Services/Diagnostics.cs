using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Tools;
using Espionage.Engine.Tripods;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Services
{
	/// <summary>
	/// Diagnostics service is responsible for providing the
	/// dev tripod, and tools. For use in debugging.
	/// </summary>
	public sealed class Diagnostics : Service
	{
		private Camera _camera;

		public override void OnReady()
		{
			_camera = Engine.Camera;
			_toolsGrouping = Library.Database.GetAll<Window>().Where( e => !e.Info.IsAbstract ).GroupBy( e => e.Group );
		}

		private object _selection;

		public object Selection
		{
			get => _selection;
			set
			{
				if ( value == null || value == _selection )
				{
					return;
				}

				_selection = value;

				var inspector = Window.Show<Inspector>();
				inspector.SelectionChanged( value );
			}
		}

		[Function, Callback( "map.unloading" )]
		private void OnMapLoad()
		{
			_selection = null;
		}

		public Entity Hovering { get; set; }
		private Collider _lastCollider;

		public override void OnUpdate()
		{
			if ( Input.GetKeyDown( KeyCode.F1 ) )
			{
				Dev.Terminal.Invoke( "dev.tripod" );
			}

			// If were in Dev Tripod
			if ( !Local.Client.Tripod.Is<DevTripod>() )
			{
				return;
			}

			// Hovering and Selection
			if ( !Input.GetMouseButton( 1 ) )
			{
				if ( !Physics.Raycast( _camera.ScreenPointToRay( Input.mousePosition ), out var hit ) )
				{
					Hovering = null;
					return;
				}

				if ( !(ImGui.IsWindowHovered() || ImGui.IsAnyItemHovered()) && Input.GetMouseButtonDown( 0 ) )
				{
					Selection = null;

					if ( hit.collider.TryGetComponent<Entity>( out var entity ) )
					{
						Selection = entity;
					}
				}

				if ( hit.collider != _lastCollider )
				{
					Hovering = null;
					if ( hit.collider.TryGetComponent<Entity>( out var entity ) )
					{
						Hovering = entity;
					}
				}
			}
			else
			{
				Hovering = null;
			}
		}

		public bool Show { get; set; }
		public bool Enabled => Local.Client.Tripod.Is<DevTripod>() || Show;

		//
		// UI
		//

		private IEnumerable<IGrouping<string, Library>> _toolsGrouping;

		[Function, Callback( "imgui.layout" )]
		private void Layout()
		{
			ImGui.DockSpaceOverViewport( ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode );
			Window.Apply( this );

			if ( !Enabled )
			{
				return;
			}

			// Active Hovering Entity Tooltip
			if ( Hovering != null )
			{
				ImGui.SetNextWindowBgAlpha( 0.7f );
				ImGui.BeginTooltip();
				{
					ImGui.Text( $"{Hovering.ClassInfo.Title}" );
					ImGui.Text( $"{Hovering.ClassInfo.Name} - [{Hovering.ClassInfo.Group}]" );
				}
				ImGui.EndTooltip();
			}

			// Main Menu Bar
			if ( ImGui.BeginMainMenuBar() )
			{
				// Left Padding
				ImGui.Dummy( new( 2, 0 ) );
				ImGui.Separator();

				// Title
				ImGui.Dummy( new( 4, 0 ) );
				ImGui.Text( "Espionage.Engine" );
				ImGui.Dummy( new( 4, 0 ) );

				ImGui.Separator();

				if ( ImGui.BeginMenu( "View" ) )
				{
					if ( ImGui.MenuItem( "Always Show Windows", string.Empty, Show ) )
					{
						Show = !Show;
					}

					ImGui.EndMenu();
				}

				if ( ImGui.BeginMenu( "Tools" ) )
				{
					// Post Processing Debug Overlays
					foreach ( var value in _toolsGrouping )
					{
						if ( ImGui.BeginMenu( value.Key ) )
						{
							foreach ( var item in value )
							{
								if ( ImGui.MenuItem( item.Title, null, Window.All.ContainsKey( item ) ) )
								{
									if ( Window.All.ContainsKey( item ) )
									{
										Window.All[item].Delete();
										continue;
									}

									Library.Create( item );
								}
							}

							ImGui.EndMenu();
						}
					}

					ImGui.EndMenu();
				}

				if ( ImGui.BeginMenu( "Graphics" ) )
				{
					Callback.Run( "dev.menu_bar.graphics" );
					ImGui.EndMenu();
				}

				if ( ImGui.BeginMenu( "Help" ) )
				{
					ImGui.EndMenu();
				}

				ImGui.EndMainMenuBar();
			}
		}
	}
}
