using Espionage.Engine.Resources;
using Espionage.Engine.Viewmodels;

namespace Espionage.Engine
{
	public class Mark23 : Holdable
	{
		protected override Viewmodel OnViewmodel( string path )
		{
			var viewmodel = Create<Viewmodel>();
			viewmodel.Components
				.Build<SimpleSway>()
				.Build<Breathe>()
				.Build<Bob>()
				.Build<Guntuck>();

			viewmodel.Visuals.Model = Model.Load( Files.Pathing.All( "models://Viewmodels" ).Random() );
			return viewmodel;
		}
	}

	public abstract class Holdable : Pickup, ISimulated
	{
		[Function( "test.holdable" ), Terminal]
		public static void CreateHoldable()
		{
			var holdable = Create<Mark23>();
			holdable.Client = Local.Client;
			holdable.Deploy();
		}

		[Function( "test.holster" ), Terminal]
		public static void HolsterHoldable()
		{
			All.Find<Mark23>()?.Holster();
		}

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
			Debugging.Log.Info( $"Deployed [{ClassInfo.Name}]" );
		}

		// Holstering

		public bool IsHolstering { get; set; }
		protected virtual float HolsterDelay { get; set; }
		protected TimeSince TimeSinceHolster { get; set; }

		public virtual bool CanHolster()
		{
			return true;
		}

		public virtual void Holster( bool dropped = false )
		{
			IsHolstering = true;
			TimeSinceHolster = 0;

			if ( HolsterDelay == 0 )
			{
				OnHolstered();
				return;
			}
		}

		protected virtual void OnHolstered()
		{
			Visuals.Enabled = false;
			IsHolstering = false;

			// Destroy Viewmodel
			Destroy( Viewmodel.gameObject );
			Viewmodel = null;
			
			Debugging.Log.Info( $"Holstered [{ClassInfo.Name}]" );
		}
	}
}
