using UnityEngine;

namespace Espionage.Engine
{
	[RequireComponent( typeof( Camera ) )]
	internal class CameraController : SingletonComponent<CameraController>
	{
		private Camera _target;

		protected override void Awake()
		{
			base.Awake();

			_target = GetComponent<Camera>();
		}

		public void Finalise( Tripod.Setup camSetup )
		{
			var trans = transform;
			trans.position = camSetup.Position;
			trans.rotation = camSetup.Rotation;
		}
	}
}
