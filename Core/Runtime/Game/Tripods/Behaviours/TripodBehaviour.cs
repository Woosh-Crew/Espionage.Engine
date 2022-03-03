namespace Espionage.Engine.Tripods
{
	public class TripodBehaviour : Behaviour, ITripod, IControls
	{
		public void Activated( ref ITripod.Setup camSetup ) { }
		public void Deactivated() {  }
		
		// Tripod
		
		void ITripod.Build( ref ITripod.Setup camSetup ) {  }
		
		// Controls
		
		void IControls.Build( ref IControls.Setup setup ) {  }
	}
}
