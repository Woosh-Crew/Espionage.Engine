using System.Collections.Generic;
using Espionage.Engine.Internal.Overlay;
using UnityEngine;

namespace Espionage.Engine.Services
{
	internal class OverlaysService : Service
	{
		public override void OnReady()
		{
			Dev.Overlay = new UnityOverlayProvider( this );
		}

		private List<GUIBlock> _stack = new();

		public void Add( string text, Vector2 size )
		{
			_stack.Add( new( text, size ) );
		}

		public override void OnUpdate()
		{
			_stack.Clear();
		}

		public struct GUIBlock
		{
			public GUIBlock( string text, Vector2 size )
			{
				this.text = text;
				this.size = size;
			}

			public string text;
			public Vector2 size;
		}


		[Function, Callback( "imgui.draw" )]
		private void OnGUI()
		{
			if ( !Dev.Overlay.Show )
			{
				return;
			}

			var scale = Screen.width / 1920 * 2;
			var currentOffset = 16f * scale;

			for ( var i = 0; i < _stack.Count; i++ )
			{
				var current = _stack[i];

				var style = new GUIStyle( GUI.skin.box ) { fontSize = 8 * scale, alignment = TextAnchor.MiddleCenter };

				GUI.Box( new( 16 * scale, currentOffset, current.size.x * scale, current.size.y * scale ), current.text, style );

				currentOffset += current.size.y * scale + 4;
			}
		}
	}
}
