using Espionage.Engine.Tools;
using Espionage.Engine.Tripods;
using ImGuiNET;
using ImGuizmoNET;
using UnityEngine;

namespace Espionage.Engine.Services
{
	/// <summary>
	/// Diagnostics service is responsible for providing the
	/// dev tripod, and tools. For use in debugging.
	/// </summary>
	public class DiagnosticsService : Service
	{
		public ILibrary Selection { get; set; }
		public Entity Hovering { get; set; }

		private Camera _camera;

		public override void OnReady()
		{
			_camera = Engine.Camera;
		}

		public override void OnUpdate()
		{
			// Use Dev Tripod
			if ( Input.GetKeyDown( KeyCode.F1 ) )
			{
				Dev.Terminal.Invoke( "dev.tripod" );
			}

			// If were in Dev Tripod
			if ( !Enabled )
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
						Window.Show<Inspector>();
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

		private Collider _lastCollider;

		private bool Enabled => Local.Client.Tripod is DevTripod;

		//
		// UI
		//

		[Function, Callback( "imgui.layout" )]
		private void Layout()
		{
			if ( !Enabled )
			{
				return;
			}

			Window.Apply( this );

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

				if ( ImGui.BeginMenu( "Tools" ) )
				{
					Callback.Run( "dev.menu_bar.tools" );
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
