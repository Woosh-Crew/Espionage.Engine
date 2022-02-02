using System.IO;
using Espionage.Engine.Services;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public class MapService : IService
	{
		public void OnReady()
		{
			using var stopwatch = Debugging.Stopwatch( "Caching Maps" );

			// Cache Asset Bundle Maps

			var path = Application.isEditor ? "Exports/Maps" : Application.dataPath;
			var extension = Library.Database.Get<Map>().Components.Get<FileAttribute>().Extension;

			foreach ( var map in Directory.GetFiles( path, $"*.{extension}", SearchOption.AllDirectories ) )
			{
				Map.Database.Add( new Map( map ) );
			}
		}
		
		public void OnShutdown() {  }
		
		public void OnUpdate() { }
	}
}
