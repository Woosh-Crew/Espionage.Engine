using System.Linq;
using Espionage.Engine;
using Espionage.Engine.Resources;
using Espionage.Engine.Steam;
using Espionage.Engine.Steam.Resources;
using Steamworks.Ugc;

[Title( "Sneaky Killer" ), Steam( 636480 )]
public class SneakyKiller : Game
{
	public override void OnReady() { }

	public override void OnShutdown() { }

	[Function( "workshop.load" ), Terminal]
	public static async void Test( string name )
	{
		var page = await Query.ItemsReadyToUse.WhereSearchText( name ).GetPageAsync( 1 );

		if ( !page.HasValue || page.Value.ResultCount == 0 )
		{
			return;
		}

		var item = page.Value.Entries.FirstOrDefault();

		var extensions = Library.Database.GetAll<Map.File>().Select( e => e.Components.Get<FileAttribute>()?.Extension ).ToArray();

		Engine.Game.Loader.Start(
			new Loader.Request( Workshop.Download( item ) ),
			new Loader.Request( () =>
			{
				var path = Files.Pathing.All( item.Directory, extensions ).FirstOrDefault();

				if ( path == null )
				{
					Dev.Log.Warning( "Path was empty" );
					return null;
				}

				if ( Map.Exists( path ) )
				{
					return Map.Find( path );
				}

				return Map.Setup( path )
					.Meta( item.Title, item.Description, item.Owner.Name )
					.Origin( "Steam Workshop" )
					.With<UGC>( new( item ) )
					.Build();
			} )
		);
	}
}
