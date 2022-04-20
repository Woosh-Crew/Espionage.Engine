using Espionage.Engine.Components;
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
			full = Files.Pathing.Absolute( "assets://" ) + Files.Pathing.Relative( "assets://", Path + "/" + Files.Pathing.Relative( "assets://", path ) );
			return Files.Pathing.Exists( full );
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
			var name = Files.Pathing.Name( Path );
			Files.Pathing.Add( name, Path );

			// Grab Maps
			if ( !Files.Pathing.Exists( $"{name}://Maps" ) )
			{
				return;
			}

			foreach ( var mapPath in Files.Pathing.All( $"{name}://Maps", Map.Extensions ) )
			{
				Map.Setup.Path( mapPath )?
					.Origin( name )
					.Meta( Files.Pathing.Name( mapPath ) )
					.Build();
			}
		}

		bool IResource.Unload()
		{
			return false;
		}
	}
}
