using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			if ( !Files.Pathing.Exists( "config://.cookies" ) )
			{
				// Nothing to load.
				return;
			}

			// This is a really shitty ini deserializer

			using var _ = Debugging.Stopwatch( "Loading Cookies" );
			var sheet = Files.Deserializer.Deserialize<string>( "config://.cookies" ).Split( '\n', StringSplitOptions.RemoveEmptyEntries );

			foreach ( var item in sheet )
			{
				// Comments (Not sure why there would be any?)
				if ( string.IsNullOrWhiteSpace( item ) || item.StartsWith( '#' ) || item.StartsWith( '[' ) )
				{
					continue;
				}

				// 0 is Index, 1 is value
				var split = item.Split( " = " );

				try
				{
					if ( Registry.ContainsKey( split[0] ) )
					{
						// This is aids
						var prop = Registry[split[0]];
						prop[null] = Converter.Convert( split[1].Trim(), prop.Type );
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

		[Terminal, Function( "cookies.save" )]
		public static void Save()
		{
			if ( Registry.Count == 0 )
			{
				// Nothing to save.
				return;
			}

			// This is a really shitty ini serializer

			using var _ = Debugging.Stopwatch( "Saving Cookies" );

			var serialized = new StringBuilder();

			foreach ( var group in Registry.GroupBy( e => e.Value.Group ) )
			{
				// Grouping / Sections
				if ( serialized.Length != 0 )
				{
					serialized.AppendLine();
				}

				serialized.AppendLine( $"[{group.Key}]" );

				foreach ( var (key, property) in group )
				{
					serialized.AppendLine( $"{key} = {property[null]}" );
				}
			}

			Files.Save( serialized.ToString(), "config://.cookies" );

			Callback.Run( "cookies.saved" );
		}
	}
}
