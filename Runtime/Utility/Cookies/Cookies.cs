using System;
using System.Collections.Generic;
using Espionage.Engine.Services;

namespace Espionage.Engine.Internal
{
	/// <summary>
	/// Cookies are responsible for saving global variables. Use this for storing
	/// the value of preferences or ConVars
	/// </summary>
	public class Cookies : Service
	{
		private static Dictionary<string, Property> Registry { get; } = new();

		public static void Register( Property prop )
		{
			Registry.Add( prop.Name, prop );
		}

		public override void OnReady()
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
					if ( Registry.ContainsKey( split[0] ) )
					{
						// This is aids
						var prop = Registry[split[0]];
						prop[null] = Converter.Convert( split[1], prop.Type );
					}
				}
				catch ( Exception e )
				{
					Debugging.Log.Exception( e );
				}
			}
		}

		public override void OnShutdown()
		{
			Save();
		}

		// Saving

		public static void Save()
		{
			if ( Registry.Count == 0 )
			{
				// Nothing to save.
				return;
			}

			using var _ = Debugging.Stopwatch( "Saving Cookies" );

			var serialized = new List<string>( Registry.Count );

			foreach ( var (key, value) in Registry )
			{
				serialized.Add( $"{key}={value[null]}" );
			}

			Files.Save( serialized, "config://.cookies" );
		}
	}
}
