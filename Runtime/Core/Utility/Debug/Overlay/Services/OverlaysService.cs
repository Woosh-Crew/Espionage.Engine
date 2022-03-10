using System.Collections.Generic;
using Espionage.Engine.Overlay;
using UnityEngine;

namespace Espionage.Engine.Services
{
	internal class OverlaysService : Service
	{
		public override void OnReady()
		{
			Debugging.Overlay = new UnityOverlayProvider( this );
		}

		private List<string> _stack = new();
		
		public override void OnUpdate()
		{
			// Check stack
		}

		public override void OnPostUpdate()
		{
			// Clear Stack
			_stack.Clear();
		}


		[Function, Callback( "imgui.draw" )]
		private void OnGUI()
		{
			if ( !Debugging.Overlay.Show )
			{
				return;
			}

			const float scale = 2;

			for ( int i = 0; i < _stack.Count; i++ )
			{
				var current = _stack[i];
				
				GUI.Box( new( 32 , Screen.height - ((64 * scale) + (32 * i ) + 8), 128 * scale, 64 * scale ) , current );
			}
		}
	}
}
