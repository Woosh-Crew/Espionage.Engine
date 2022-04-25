using System.IO;
using Espionage.Engine.Components;
using Espionage.Engine.IO;
using Espionage.Engine.Resources;

namespace Espionage.Engine
{
	[Library( "res.mod" ), Group( "Mods" ), Path( "mods", "assets://Mods" )]
	public sealed class Mod : IResource
	{
		public Library ClassInfo { get; }
		public Components<Mod> Components { get; }

		// Meta

		public string Path { get; private set; }

		public Mod()
		{
			ClassInfo = Library.Register( this );
			Components = new( this );
		}

		public bool Exists( string path, out string full )
		{
			var assets = Files.Pathing( "assets://" );
			var absolute = assets.Absolute();

			full = absolute + assets.Relative( Path + "/" + assets.Relative( path ) );
			return Files.Pathing( full ).Exists();
		}

		// Resource

		int IResource.Identifier { get; set; }
		bool IResource.Persistant { get; set; }

		void IResource.Setup( string path )
		{
			Path = path;
		}

		void IResource.Load()
		{
			var name = Files.Pathing( Path ).Name();
			Pathing.Add( name, Path );

			// Grab Maps

			var mapPathing = Files.Pathing( $"{name}://Maps" );

			if ( !mapPathing.Exists() )
			{
				return;
			}

			foreach ( var file in mapPathing.All( SearchOption.AllDirectories, Map.Extensions ) )
			{
				Map.Setup.Path( file )?
					.Origin( name )
					.Meta( Files.Pathing( file ).Name() )
					.Build();
			}
		}

		bool IResource.Unload()
		{
			return false;
		}
	}
}
