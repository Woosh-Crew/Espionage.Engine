using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class Guntuck : Viewmodel.Modifier
	{
		public float Damping { get; set; } = 8;
		public float Roll { get; set; } = 90;

		// Logic

		private float _dampedOffset;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var muzzlePos = setup.Position + setup.Rotation.Forward() * 0.8f;
			var muzzleRot = Entity.Rotation;

			var distance = Vector3.Distance( muzzlePos, setup.Position );
			var start = muzzlePos + muzzleRot.Backward() * distance;
			var direction = muzzleRot.Forward();

			var hit = Trace.Ray( start, direction, distance ).Ignore( "Viewmodel", "Pawn" ).Run( out var info );

			_dampedOffset = _dampedOffset.LerpTo( -(hit ? info.Distance - distance : 0), Damping * Time.deltaTime );

			Rotation *= Quaternion.AngleAxis( _dampedOffset * Roll, Vector3.forward );
			Position += setup.Rotation * new Vector3( 0, -_dampedOffset / 1.7f, -_dampedOffset );
		}
	}
}
