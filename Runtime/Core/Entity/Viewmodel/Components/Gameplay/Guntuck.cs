using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class Guntuck : Viewmodel.Modifier
	{
		private float _dampedOffset;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var muzzlePos = setup.Position + setup.Rotation.Forward() * 0.8f;
			var muzzleRot = Entity.Rotation;

			var distance = Vector3.Distance( muzzlePos, setup.Position );
			var start = muzzlePos + muzzleRot.Backward() * distance;

			var hit = Physics.Raycast( new( start, muzzleRot.Forward() ), out var info, distance, ~LayerMask.GetMask( "Viewmodel", "Pawn" ), QueryTriggerInteraction.Ignore );

			_dampedOffset = _dampedOffset.LerpTo( -(hit ? info.distance - distance : 0), damping * Time.deltaTime );

			Position += setup.Rotation * new Vector3( 0, -_dampedOffset / 2, -_dampedOffset );
		}

		// Fields 

		private float damping = 8;
	}
}
