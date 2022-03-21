using Espionage.Engine.Tripods;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Services
{
	public class DiagnosticsService : Service
	{
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

			if ( !Input.GetMouseButton( 1 ) && Physics.Raycast( _camera.ScreenPointToRay( Input.mousePosition ), out var hit ) )
			{
				if ( hit.collider != _lastCollider )
				{
					_ent = null;
					if ( hit.collider.TryGetComponent<Entity>( out var entity ) )
					{
						_ent = entity;
					}
				}
			}
		}

		private Collider _lastCollider;
		private Entity _ent;

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

			// Active Hovering Entity Tooltip
			if ( _ent != null )
			{
				ImGui.SetNextWindowBgAlpha( 0.7f );
				ImGui.BeginTooltip();
				{
					ImGui.Text( $"{_ent.ClassInfo.Title}" );
					ImGui.Text( $"{_ent.ClassInfo.Name} - [{_ent.ClassInfo.Group}]" );
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
