using System.Linq;
using Espionage.Engine.Services;

namespace Espionage.Engine.Resources
{
	public sealed partial class Assets : Service
	{
		public override void OnReady()
		{
			base.OnReady();

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
			base.OnUpdate();

			// Watch asset for change
			// Unload asset after 1 minute of inactive use
		}

		public override void OnShutdown()
		{
			base.OnShutdown();

			// Stop watching asset for updates
			// Unload any currently loaded assets
		}
	}
}
