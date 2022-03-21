﻿using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Services
{
	public class DevService : Service
	{
		public override void OnUpdate()
		{
			// Use Dev Tripod
			if ( Input.GetKeyDown( KeyCode.F1 ) )
			{
				Dev.Terminal.Invoke( "dev.tripod" );
			}

			// Open Terminal
			if ( Input.GetKeyDown( KeyCode.F3 ) )
			{
				Dev.Terminal.Invoke( "dev.noclip" );
			}

			// Open Terminal
			if ( Input.GetKeyDown( KeyCode.F2 ) )
			{
				Enabled = !Enabled;
			}
		}

		//
		// UI
		//

		public bool Enabled { get; set; }

		[Function, Callback( "imgui.layout" )]
		private void Layout()
		{
			if ( !Enabled )
			{
				return;
			}

			if ( ImGui.BeginMainMenuBar() )
			{
				if ( ImGui.BeginMenu( "Files" ) )
				{
					ImGui.MenuItem( "Main menu bar" );
					ImGui.EndMenu();
				}

				if ( ImGui.BeginMenu( "Maps" ) )
				{
					ImGui.EndMenu();
				}

				ImGui.EndMainMenuBar();
			}

			TerminalUI();
		}

		// Terminal UI

		private string _input = string.Empty;

		private void TerminalUI()
		{
			// Terminal
			if ( !ImGui.Begin( "Terminal" ) )
			{
				// End if were not valid
				ImGui.End();
				return;
			}

			ImGui.InputTextWithHint( string.Empty, "Enter Command...", ref _input, 128 );
			if ( ImGui.Button( "Submit" ) )
			{
				Dev.Terminal.Invoke( _input );
			}
		}
	}
}
