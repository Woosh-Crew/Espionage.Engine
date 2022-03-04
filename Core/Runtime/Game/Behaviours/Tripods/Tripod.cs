using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Group( "Tripods" )]
	public class Tripod : Behaviour, ITripod, IControls
	{
		public Transform Visuals => visuals;

		// Tripod

		public virtual void Activated( ref ITripod.Setup camSetup ) { }
		public virtual void Deactivated() { }

		void ITripod.Build( ref ITripod.Setup camSetup )
		{
			camSetup.Viewer = Visuals;

			OnBuildTripod( ref camSetup );
		}

		protected virtual void OnBuildTripod( ref ITripod.Setup setup ) { }

		// Controls

		void IControls.Build( ref IControls.Setup setup )
		{
			OnBuildControls( ref setup );
		}

		protected virtual void OnBuildControls( ref IControls.Setup setup )
		{
			setup.ViewAngles += new Vector3( -setup.MouseDelta.y, setup.MouseDelta.x, 0 );
			setup.ViewAngles.x = Mathf.Clamp( setup.ViewAngles.x, -88, 88 );
		}

		// Fields

		private Transform visuals;
	}
}
