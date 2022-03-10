using System.Collections.Generic;
using Espionage.Engine.Services;
using UnityEngine;

namespace Espionage.Engine.Overlay
{
	internal class UnityOverlayProvider : IDebugOverlayProvider
	{
		public UnityOverlayProvider( OverlaysService service )
		{
			Service = service;
		}

		private OverlaysService Service { get; }

		//
		// Overlay
		//
		
		public bool Show { get; set; } = true;

		public void Text( Vector2 pos, string text )
		{
			if ( !Show )
			{
				return;
			}
		}

		public void Text( Vector3 pos, string text )
		{
			if ( !Show )
			{
				return;
			}
		}

		public void Box( Rect rect, string text )
		{
			if ( !Show )
			{
				return;
			}
		}
	}
}
