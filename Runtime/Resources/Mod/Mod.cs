using System.IO;
using Espionage.Engine.Components;
using Espionage.Engine.IO;
using Espionage.Engine.Resources;

namespace Espionage.Engine
{
	[Library( "res.mod" ), Group( "Mods" ), Path( "mods", "assets://Mods" )]
	public sealed class Mod : IAsset
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

		// Resource

		public Resource Resource { get; set; }

		void IAsset.Setup( Pathing path )
		{
			Path = path;
			Resource.Persistant = true;
		}

		void IAsset.Load()
		{
			var name = Files.Pathing( Path ).Name();
			Pathing.Add( name, Path );

			// Add Models

			if ( Files.Pathing( $"{name}://Models" ).Exists() )
			{
				Pathing.Add( "models", $"{name}://Models" );
			}

			// Grab Maps

			var mapPathing = Files.Pathing( $"{name}://Maps" );

			if ( !mapPathing.Exists() )
			{
				return;
			}

			foreach ( var file in mapPathing.All( SearchOption.AllDirectories, Map.Extensions ) )
			{
				Map.Setup.Path( file )?.Origin( name ).Meta( Files.Pathing( file ).Name() ).Build();
			}
		}

		void IAsset.Unload() { }

		public IAsset Clone()
		{
			return null;
		}
	}
}
