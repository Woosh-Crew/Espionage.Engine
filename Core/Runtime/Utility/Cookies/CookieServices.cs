using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Services
{
	/// <summary>
	/// Cookies are responsible for saving
	/// global variables.
	/// </summary>
	public class CookieServices : Service
	{
		public Dictionary<string, Property> Registry { get; } = new();

		// States

		public override void OnReady()
		{
			// Get all Cookies
			foreach ( var item in Library.Database.All.SelectMany( e => e.Properties.All.Where( property => property.Components.Has<CookieAttribute>() ) ) )
			{
				Registry.Add( item.Name, item );
			}

			Load();
		}

		public override void OnShutdown()
		{
			Save();
		}

		// Serialization

		private void Load()
		{
			if ( !Files.Exists( "config://.cookies" ) )
			{
				// Nothing to load.
				return;
			}

			using var _ = Debugging.Stopwatch( "Loading Cookies" );

			var sheet = Files.Deserialize<string>( "config://.cookies" ).Split( '\n' );

			foreach ( var item in sheet )
			{
				var split = item.Split( '=' );

				// This is aids
				var prop = Registry[split[0]];
				prop[null] = Converter.Convert( split[1], prop.Type );
				Debugging.Log.Info( $"Property [{prop.Name}] = {prop[null]}" );
			}
		}

		public void Save()
		{
			if ( Registry.Count == 0 )
			{
				// Nothing to save.
				return;
			}

			var serialized = new List<string>( Registry.Count );

			foreach ( var (key, value) in Registry )
			{
				serialized.Add( $"{key}={value[null]}" );
			}

			Files.Save( serialized, "config://.cookies" );
		}
	}
}
