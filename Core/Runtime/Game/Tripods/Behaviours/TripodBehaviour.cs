using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Group( "Tripods" ), Title( "Virtual Tripod" )]
	public class TripodBehaviour : Behaviour, ITripod, IControls
	{
		public ITripod Active { get; protected set; }

		protected override void OnAwake()
		{
			Active = tripod.Create();
		}

		public void Activated( ref ITripod.Setup camSetup )
		{
			Active?.Activated( ref camSetup );
		}

		public void Deactivated()
		{
			Active?.Deactivated();
		}

		// Tripod

		void ITripod.Build( ref ITripod.Setup setup )
		{
			// Set the Viewer here. The Tripod
			// Should be responsible for removing it.
			setup.Viewer = visuals;

			Active?.Build( ref setup );
		}

		protected virtual void OnBuildTripod( ref ITripod.Setup setup ) { }

		// Controls

		void IControls.Build( ref IControls.Setup setup )
		{
			(Active as IControls)?.Build( ref setup );
			OnBuildControls( ref setup );
		}

		protected virtual void OnBuildControls( ref IControls.Setup setup ) { }

		// Fields

		[SerializeField]
		private Book<Tripod> tripod;

		[SerializeField]
		private Transform visuals;
	}
}
