using System.Linq;
using Espionage.Engine.Services;

namespace Espionage.Engine.Resources
{
	public sealed partial class Assets : Service
	{
		public override void OnReady()
		{
			// Get reference pathing to all resources
			// Load default resources (Maps & Mods)

			foreach ( var pathing in Library.Database.GetAll<IAsset>().Select( e => e.Components.Get<PathAttribute>() ) )
			{
				foreach ( var file in Files.Pathing( $"{pathing.ShortHand}://" ).All() )
				{
					Registered.Fill( file.Virtual().Normalise() );
				}
			}
		}

		public override void OnUpdate()
		{
			// Watch asset for change
			// Unload asset after 1 minute of inactive use

			Sweep();
		}

		public override void OnShutdown()
		{
			// Stop watching asset for updates
			// Unload any currently loaded assets

			foreach ( var resource in Registered )
			{
				resource.Unload( true );
			}
		}

		// Sweep

		private RealTimeSince _timeSinceSweep;
		private const int _timeBetweenSweeps = 60;

		private void Sweep()
		{
			if ( !(_timeSinceSweep > _timeBetweenSweeps) )
			{
				return;
			}

			_timeSinceSweep = 0;

			foreach ( var resource in Registered )
			{
				if ( resource.Instances?.Count == 0 && !resource.Persistant )
				{
					Debugging.Log.Info( $"No Instances of [{resource.Path}], Unloading" );
					resource.Unload( false );
				}
			}
		}
	}
}
