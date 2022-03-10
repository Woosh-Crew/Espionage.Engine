using UnityEngine;

namespace Espionage.Engine.Services
{
	internal class TerminalService : Service
	{
		public override void OnUpdate()
		{
			if ( Input.GetKeyDown( KeyCode.F3 ) )
			{
				_enabled = !_enabled;
			}			
		}

		private bool _enabled;

		[Function, Callback( "imgui.draw" )]
		private void OnGUI()
		{
			if ( !_enabled )
				return;
			
			// Draw Terminal UI
			GUI.Box( new( 32, 32, 128, 64 ), "Test" );
		}
	}
}
