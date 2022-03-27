using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class Guntuck : Viewmodel.Modifier
	{
		private float _dampedOffset;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var distance = Vector3.Distance( muzzle.position, setup.Position );
			var start = muzzle.position + muzzle.rotation * Vector3.back * distance;

			var hit = Physics.Raycast( new( start, muzzle.rotation * Vector3.forward ), out var info, distance, ~LayerMask.GetMask( "Viewmodel", "Pawn" ), QueryTriggerInteraction.Ignore );

			_dampedOffset = _dampedOffset.LerpTo( -(hit ? info.distance - distance : 0), damping * Time.deltaTime );

			Position += setup.Rotation * new Vector3( 0, -_dampedOffset / 2, -_dampedOffset );
		}

		// Fields 

		[SerializeField]
		private float damping = 8;

		[SerializeField]
		private Transform muzzle;
	}
}
