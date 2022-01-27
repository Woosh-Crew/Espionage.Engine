using System;
using UnityEngine;

namespace Espionage.Engine
{
	[RequireComponent( typeof( Camera ), typeof( AudioListener ) )]
	internal class CameraController : SingletonComponent<CameraController>
	{
		private Camera _target;

		protected override void Awake()
		{
			base.Awake();

			gameObject.tag = "MainCamera";
			_target = GetComponent<Camera>();
		}

		public void Finalise( Tripod.Setup camSetup )
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
