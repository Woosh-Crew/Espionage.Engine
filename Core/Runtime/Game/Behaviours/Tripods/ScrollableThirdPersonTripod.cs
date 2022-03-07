using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.third_person_scrollable" )]
	public class ScrollableThirdPersonTripod : ThirdPersonTripod
	{
		private float _targetDistance;
		private float _smoothedDistance;

		public override void Activated( ref ITripod.Setup camSetup )
		{
			base.Activated( ref camSetup );

			_targetDistance = distance;
			_smoothedDistance = distance;
		}

		protected override void OnBuildControls( ref IControls.Setup setup )
		{
			base.OnBuildControls( ref setup );

			_targetDistance -= setup.MouseWheel * 4;
			_targetDistance = Mathf.Clamp( _targetDistance, minDistance, maxDistance );

			_smoothedDistance = _smoothedDistance.LerpTo( _targetDistance, damping * Time.deltaTime );
			distance = _smoothedDistance;

		}

		// Properties

		[SerializeField]
		private float minDistance = 1;

		[SerializeField]
		private float maxDistance = 15;

		[SerializeField]
		private float damping = 15;
	}
}
