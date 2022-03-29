using System;

namespace Espionage.Engine
{
	public abstract class Holdable : Pickup, ISimulated
	{
		/// <summary>The given viewmodel for a holdable</summary>
		public Viewmodel Viewmodel { get; set; }

		public virtual void Simulate( Client cl ) { }
		public virtual void PostCameraSetup( ref Tripod.Setup setup ) { }


		/// <summary>If a given holdable is deploying</summary>
		public bool IsDeploying { get; set; }

		/// <summary>If a given weapon can be deployed</summary>
		public virtual bool CanDeploy()
		{
			return true;
		}

		/// <summary>Deploys a given holdable</summary>
		public virtual void Deploy() { }

		/// <summary>Call back for when holdable deployed</summary>
		protected virtual void OnDeployed() { }

		/// <summary>If a given holdable is currently holstering</summary>
		public bool IsHolstering { get; set; }

		/// <summary>If a given holdable can be holstered</summary>
		public virtual bool CanHolster()
		{
			return true;
		}

		/// <summary>Holsters the given holdable</summary>
		public virtual void Holster( bool dropped = false ) { }

		/// <summary>Call back for when a given holdable is holstered</summary>
		protected virtual void OnHolstered() { }
	}
}
