using System.Linq;
using Espionage.Engine.IO;
using Espionage.Engine.Services;

namespace Espionage.Engine.Resources
{
	public sealed partial class Resource : Service
	{
		public override void OnReady()
		{
			base.OnReady();

			// Get reference pathing to all resources
			// Load default resources (Maps & Mods)

			foreach ( var pathing in Library.Database.GetAll<IResource>().Select( e => e.Components.Get<PathAttribute>() ) )
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

			// Watch resource for change
			// Unload resource after 1 minute of inactive use
		}

		public override void OnShutdown()
		{
			base.OnShutdown();

			// Stop watching resources for updates
			// Unload any currently loaded resources
		}

		// Data

		public class Reference
		{
			public IResource Resource { get; set; }

			public Reference( Pathing path )
			{
				Path = path;
				Identifier = path.Hash();
			}

			~Reference()
			{
				Resource = null;
			}

			public Pathing Path { get; }
			public int Identifier { get; }

			public override string ToString()
			{
				return $"loaded:[{Resource != null}] path:[{Path}]";
			}
		}
	}
}
