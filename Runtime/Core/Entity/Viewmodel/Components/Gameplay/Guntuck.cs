using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class Guntuck : Viewmodel.Modifier
	{
		protected float _dampedOffset;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var start = end.position + end.rotation * Vector3.back * Vector3.Distance( end.position, setup.Position );
			var direction = end.rotation * Vector3.forward;
			var distance = Vector3.Distance( setup.Position, end.position );

			var ray = new Ray( start, direction );
			var hit = Physics.Raycast( ray, out var info, distance, ~LayerMask.GetMask( "Viewmodel", "Pawn" ) );

			var offset = hit ? info.distance - distance : 0;

			_dampedOffset = _dampedOffset.LerpTo( -offset, 8 * Time.deltaTime );

			transform.position += setup.Rotation * Vector3.back * _dampedOffset;
			transform.position += setup.Rotation * Vector3.down * _dampedOffset / 2;
		}


		// Fields 

		[SerializeField]
		private Transform end;
	}
}
