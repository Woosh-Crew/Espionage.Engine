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
	public class DiagnosticsService : Service
	{
		private Camera _camera;

		public override void OnReady()
		{
			_camera = Engine.Camera;
			_toolsGrouping = Library.Database.GetAll<Window>().Where( e => !e.Class.IsAbstract ).GroupBy( e => e.Group );
		}

		public ILibrary Selection
		{
			get => _selection;
			set
			{
				if ( value == null )
				{
					return;
				}

				Window.Show<Inspector>();
				_selection = value;
			}
		}

		public Entity Hovering { get; set; }

		public override void OnUpdate()
		{
			// Use Dev Tripod
			if ( Input.GetKeyDown( KeyCode.F1 ) )
			{
				Dev.Terminal.Invoke( "dev.tripod" );
			}

			// Use Dev Tripod
			if ( Input.GetKeyDown( KeyCode.BackQuote ) )
			{
				if ( !Enabled )
				{
					Dev.Terminal.Invoke( "dev.tripod" );
				}

				var terminal = Window.Show<Terminal>();
				terminal.Focus = true;
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

		public bool Enabled => Local.Client.Tripod is DevTripod;

		//
		// UI
		//

		private IEnumerable<IGrouping<string, Library>> _toolsGrouping;
		private ILibrary _selection;

		[Function, Callback( "imgui.layout" )]
		private void Layout()
		{
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
