using System.Linq;
using Espionage.Engine.IO;
using Espionage.Engine.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Maps
{
	[Order( 150 )] // Load after everything
	internal class MapService : Service
	{
		public override void OnReady()
		{
			Map.Binder provider = Application.isEditor ? new EditorSceneMapProvider() : new BuildIndexMapProvider( 0 );

			// Get main scene at start, that isn't engine layer.
			for ( var i = 0; i < SceneManager.sceneCount; i++ )
			{
				var scene = SceneManager.GetSceneAt( i );

				if ( scene.name != Engine.Scene.name )
				{
					SceneManager.SetActiveScene( scene );
					break;
				}
			}

			Map.Current = new( provider, "id:init_map" );
			Callback.Run( "map.loaded" );

			// Cache Maps

			for ( var i = 0; i < SceneManager.sceneCountInBuildSettings; i++ )
			{
				var scene = SceneManager.GetSceneByBuildIndex( i );
				Map.Setup.Index( i ).Meta( scene.name ).Origin( "Game" ).Build();
			}

			var pathing = Files.Pathing( "maps://" ).Absolute();

			if ( !pathing.Exists() )
			{
				// No Maps in the project
				return;
			}

			var extensions = Library.Database.GetAll<Map.File>().Select( e => e.Components.Get<FileAttribute>()?.Extension ).ToArray();
			foreach ( var item in pathing.All( extensions ) )
			{
				Pathing path = item;

				Map.Setup.Path( item )?
					.Meta( path.Name( false ).ToTitleCase() )
					.Origin( "Game" )
					.Build();
			}
		}
	}
}
