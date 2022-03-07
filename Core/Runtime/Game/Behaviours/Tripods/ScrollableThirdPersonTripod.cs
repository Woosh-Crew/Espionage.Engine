using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.third_person_scrollable" )]
	public class ScrollableThirdPersonTripod : ThirdPersonTripod
	{
		protected override void OnBuildControls( ref IControls.Setup setup )
		{
			base.OnBuildControls( ref setup );

			distance -= setup.MouseWheel * 4;
			distance = Mathf.Clamp( distance, minDistance, maxDistance );
		}

		// Properties

		[SerializeField]
		private float minDistance = 1;

		[SerializeField]
		private float maxDistance = 15;
	}
}
