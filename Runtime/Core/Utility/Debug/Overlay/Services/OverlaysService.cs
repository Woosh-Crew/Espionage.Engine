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

		public void Add( string text )
		{
			_stack.Add( text );
		}

		public override void OnUpdate()
		{
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

			for ( var i = 0; i < _stack.Count; i++ )
			{
				var current = _stack[i];
				var height = 16;

				GUI.Box( new( 32,
						32 + i * (height * 2 + 8),
						64 * scale,
						height * scale ),
					current );
			}
		}
	}
}
