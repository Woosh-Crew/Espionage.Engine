using Espionage.Engine.Resources;

namespace Espionage.Engine
{
	public abstract class Holdable : Pickup, ISimulated
	{
		public virtual void Simulate( Client cl )
		{
			if ( TimeSinceDeployment >= DeployDelay && IsDeploying )
			{
				OnDeployed();
			}

			if ( TimeSinceHolster >= HolsterDelay && IsHolstering )
			{
				OnHolstered();
			}
		}

		public virtual void PostCameraSetup( ref Tripod.Setup setup ) { }

		// Viewmodel

		public Viewmodel Viewmodel { get; set; }

		protected virtual Viewmodel OnViewmodel( string path )
		{
			var vm = Create<Viewmodel>();
			vm.Visuals.Model = Model.Load( path );
			vm.Client = Client;
			return vm;
		}

		// Deploying

		public bool IsDeploying { get; set; }
		protected virtual float DeployDelay { get; set; }
		protected TimeSince TimeSinceDeployment { get; set; }

		public virtual void Deploy()
		{
			Visuals.Enabled = true;

			// Create Viewmodel

			if ( Client == Local.Client )
			{
				Viewmodel = OnViewmodel( "v_mk23.umdl" );
			}

			// Start Deploying
			IsDeploying = true;
			TimeSinceDeployment = 0;

			if ( DeployDelay == 0 )
			{
				OnDeployed();
				return;
			}

			// Run Animations
		}

		protected virtual void OnDeployed()
		{
			IsDeploying = false;
		}

		// Holstering

		public bool IsHolstering { get; set; }
		protected virtual float HolsterDelay { get; set; }
		protected TimeSince TimeSinceHolster { get; set; }

		public virtual bool CanHolster()
		{
			return true;
		}

		public virtual void Holster( bool dropped = false ) { }

		protected virtual void OnHolstered()
		{
			Visuals.Enabled = false;

			// Destroy Viewmodel
			Destroy( Viewmodel );
		}
	}
}
