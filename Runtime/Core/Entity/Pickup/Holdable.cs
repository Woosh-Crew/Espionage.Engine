using System;

namespace Espionage.Engine
{
	public class Holdable : Pickup, ISimulated
	{
		public virtual void Simulate( Client cl )
		{
			if ( IsDeploying && timeSinceDeployment >= (DeployedBefore ? DeployDelay : FirstTimeDeployDelay) )
			{
				OnDeployed();
			}

			if ( IsHolstering && timeSinceHolster >= HolsterDelay )
			{
				OnHolstered();
			}
		}

		protected override void OnDelete()
		{
			if ( Viewmodel != null )
			{
				DestroyViewmodel();
			}

			OnDeploy = null;
			OnHolster = null;
		}

		public virtual void PostCameraSetup( ref Tripod.Setup setup ) { }

		//
		// Deploy
		//

		public bool IsDeploying { get; set; }
		public bool DeployedBefore { get; set; }

		// Delays
		public virtual float FirstTimeDeployDelay { get; }
		public virtual float DeployDelay { get; }

		protected TimeSince timeSinceDeployment;

		public virtual bool CanDeploy()
		{
			return !(IsDeploying || IsHolstering) && Carrier != null;
		}

		public virtual bool Deploy()
		{
			if ( !CanDeploy() )
			{
				return false;
			}

			Enabled = true;

			IsDeploying = true;
			timeSinceDeployment = 0;

			if ( DeployDelay == 0 )
			{
				OnDeployed();
			}

			// Animator?.RunEvent( !DeployedBefore ? "first_deploy" : "deploy" );
			return true;
		}

		public Action OnDeploy { get; set; }

		protected virtual void OnDeployed()
		{
			IsDeploying = false;
			OnDeploy?.Invoke();
			OnDeploy = null;

			DeployedBefore = true;
		}

		//
		// Holster
		//

		public bool IsHolstering { get; set; }
		public virtual float HolsterDelay { get; }

		protected TimeSince timeSinceHolster;

		public virtual bool CanHolster()
		{
			return !(IsDeploying || IsHolstering) && Carrier != null;
		}

		public virtual bool Holster( bool dropped = false )
		{
			if ( !CanHolster() )
			{
				return false;
			}

			if ( HolsterDelay == 0 )
			{
				OnHolstered();
				return true;
			}

			IsHolstering = true;
			timeSinceHolster = 0;

			return true;
		}

		public Action OnHolster { get; set; }

		protected virtual void OnHolstered()
		{
			OnHolster?.Invoke();
			OnHolster = null;
			IsHolstering = false;

			Enabled = false;
		}

		//
		// Viewmodel
		//

		public Viewmodel Viewmodel { get; protected set; }

		public virtual void SetViewmodel( string path )
		{
			if ( string.IsNullOrEmpty( path ) )
			{
				return;
			}
		}

		public virtual void DestroyViewmodel()
		{
			if ( Viewmodel != null )
			{
				Destroy( Viewmodel );
			}

			Viewmodel = null;
		}
	}
}
