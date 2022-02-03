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
		}

		public void Finalise( ICamera.Setup camSetup )
		{
			var trans = transform;
			trans.position = camSetup.Position;
			trans.rotation = camSetup.Rotation;

			_target.fieldOfView = camSetup.FieldOfView;
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere( transform.position, 0.8f );
		}
	}
}
