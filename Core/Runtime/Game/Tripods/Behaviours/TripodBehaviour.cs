using UnityEngine;

namespace Espionage.Engine.Tripods
{
	public class TripodBehaviour : Behaviour, ITripod, IControls
	{
		public Tripod Active { get; private set; }

		protected override void OnAwake()
		{
			base.OnAwake();

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
			((ITripod)Active)?.Build( ref setup );
		}

		// Controls

		void IControls.Build( ref IControls.Setup setup )
		{
			((IControls)Active)?.Build( ref setup );
		}

		// Fields

		[SerializeField]
		private string tripod;
	}
}
