using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class Guntuck : Viewmodel.Modifier
	{
		private float _dampedOffset;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var distance = Vector3.Distance( end.position, setup.Position );
			var start = end.position + end.rotation * Vector3.back * distance;

			var hit = Physics.Raycast( new( start, end.rotation * Vector3.forward ), out var info, distance, ~LayerMask.GetMask( "Viewmodel", "Pawn" ) );

			_dampedOffset = _dampedOffset.LerpTo( -(hit ? info.distance - distance : 0), 8 * Time.deltaTime );

			transform.position += setup.Rotation * new Vector3( 0, -_dampedOffset / 2, -_dampedOffset );

		}

		// Fields 

		[SerializeField]
		private Transform end;
	}
}
