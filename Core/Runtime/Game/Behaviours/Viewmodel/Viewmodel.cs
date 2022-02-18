using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public sealed class Viewmodel : Behaviour
	{
		public static List<Viewmodel> All { get; } = new();

		protected override void OnAwake()
		{
			All.Add( this );

			foreach ( var render in GetComponentsInChildren<Renderer>() )
			{
				render.gameObject.layer = LayerMask.GetMask( "Viewmodel" );
			}
		}

		protected override void OnDelete()
		{
			All.Remove( this );
		}

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			var trans = transform;
			trans.localPosition = setup.Position;
			trans.localRotation = setup.Rotation;
		}
	}
}
