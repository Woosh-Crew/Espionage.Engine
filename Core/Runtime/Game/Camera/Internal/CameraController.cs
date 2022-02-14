using System;
using UnityEngine;

namespace Espionage.Engine
{
	[RequireComponent( typeof( Camera ), typeof( AudioListener ), typeof( FlareLayer ) )]
	internal class CameraController : MonoBehaviour
	{
		private Camera _target;

		private void Awake()
		{
			gameObject.tag = "MainCamera";
			_target = GetComponent<Camera>();
			_target.depth = 2;
		}

		public void Finalise( ICamera.Setup camSetup )
		{
			var trans = transform;
			trans.localPosition = camSetup.Position;
			trans.localRotation = camSetup.Rotation;

			_target.fieldOfView = camSetup.FieldOfView;
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere( transform.position, 0.8f );

			// This is hacky.. But who cares
			Callback.Run( "debug.gizmos" );
		}
	}
}
