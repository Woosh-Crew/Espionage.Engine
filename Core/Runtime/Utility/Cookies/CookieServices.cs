using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Services
{
	/// <summary>
	/// Cookies are responsible for saving
	/// global variables. Use this for storing
	/// the value of preferences or ConVars
	/// </summary>
	public class CookieServices : Service
	{
		private Dictionary<string, Property> Registry { get; } = new();

		public void Register( Property prop )
		{
			Registry.Add( prop.Name, prop );
		}

		// States

		public override void OnReady()
		{
			foreach ( var property in Library.Global.Properties.All.Where( e => e.Components.Has<CookieAttribute>() ) )
			{
				Register( property );
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

				try
				{
					// This is aids
					var prop = Registry[split[0]];
					prop[null] = Converter.Convert( split[1], prop.Type );
				}
				catch ( Exception e )
				{
					Debugging.Log.Exception( e );
				}
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
