using System;

namespace Espionage.Engine
{
	public abstract class Holdable : Pickup, ISimulated
	{
		public Viewmodel Viewmodel { get; set; }

		public virtual void Simulate( Client cl ) { }
		public virtual void PostCameraSetup( ref Tripod.Setup setup ) { }

		//
		// Deploy
		//

		public bool IsDeploying { get; set; }

		public virtual bool CanDeploy()
		{
			return true;
		}

		public virtual void Deploy() { }
		protected virtual void OnDeployed() { }

		//
		// Holster
		//

		public bool IsHolstering { get; set; }

		public virtual bool CanHolster()
		{
			return true;
		}

		public virtual void Holster( bool dropped = false ) { }
		protected virtual void OnHolstered() { }
	}
}
