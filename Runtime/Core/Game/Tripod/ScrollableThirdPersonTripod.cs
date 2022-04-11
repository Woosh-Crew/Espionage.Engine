using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.third_person_scrollable" )]
	public class ScrollableThirdPersonTripod : ThirdPersonTripod
	{
		public float MinDistance { get; set; } = 1;
		public float MaxDistance { get; set; } = 15;
		public float Damping { get; set; } = 15;

		// Logic

		private float _targetDistance;
		private float _smoothedDistance;

		public override void Activated( ref Setup camSetup )
		{
			base.Activated( ref camSetup );

			_targetDistance = Distance;
			_smoothedDistance = Distance;
		}

		protected override void OnBuildControls( Controls.Setup setup )
		{
			base.OnBuildControls( setup );

			_targetDistance -= setup.Mouse.Wheel * 4;
			_targetDistance = Mathf.Clamp( _targetDistance, MinDistance, MaxDistance );

			_smoothedDistance = _smoothedDistance.LerpTo( _targetDistance, Damping * Time.deltaTime );
			Distance = _smoothedDistance;

		}
	}
}
