using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class VelocityOffset : Viewmodel.Modifier
	{
		public float Intensity { get; set; } = 1f;

		private Vector3 _dampedOffset;

		public override void PostCameraSetup( ref Tripod.Setup setup )
		{
			var offset = Local.Pawn.Velocity.WithY( 0 );
			_dampedOffset = _dampedOffset.LerpTo( offset * Intensity, 6 * Time.deltaTime );

			Position -= _dampedOffset * 1.1f;
		}
	}
}
