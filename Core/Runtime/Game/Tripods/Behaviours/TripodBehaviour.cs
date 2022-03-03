using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Group( "Tripods" )]
	public class TripodBehaviour : Behaviour, ITripod, IControls
	{
		public ITripod Active { get; set; }

		protected override void OnAwake()
		{
			Active = Library.Database.Create<Tripod>( tripod );
		}

		public void Activated( ref ITripod.Setup camSetup )
		{
			Active.Activated( ref camSetup );
		}

		public void Deactivated()
		{
			Active.Deactivated();
		}

		// Tripod

		void ITripod.Build( ref ITripod.Setup setup )
		{
			// Set the Viewer here. The Tripod
			// Should be responsible for removing it.
			setup.Viewer = visuals;

			Active?.Build( ref setup );
		}

		// Controls

		void IControls.Build( ref IControls.Setup setup )
		{
			(Active as IControls)?.Build( ref setup );
		}

		// Fields

		[SerializeField]
		private string tripod;

		[SerializeField]
		private Transform visuals;
	}
}
