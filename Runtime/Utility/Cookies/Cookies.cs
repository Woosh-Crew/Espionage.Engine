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
		private static Dictionary<string, Reference> Registry { get; } = new();

		public static void Register( Reference cookie )
		{
			Registry.Add( cookie.Property.Name, cookie );
		}

		public override void OnReady()
		{
			var files = Registry.Values.GroupBy( e => e.File );

			// Groupings
			foreach ( var file in files )
			{
				if ( !Files.Pathing.Exists( file.Key ) )
				{
					// Nothing to load.
					return;
				}

				using var _ = Dev.Stopwatch( $"Loading Cookies [{file.Key}]" );

				// This is a really shitty ini deserializer
				var sheet = Files.Serialization.Deserialize<string>( "serializer.string", file.Key ).Split( '\n', StringSplitOptions.RemoveEmptyEntries );

				foreach ( var item in sheet )
				{
					// Comments (Not sure why there would be any?)
					if ( string.IsNullOrWhiteSpace( item ) )
					{
						continue;
					}

					// 0 is Index, 1 is value
					var split = item.Split( " = " );

					if ( Registry.ContainsKey( split[0] ) )
					{
						// This is aids
						var prop = Registry[split[0]].Property;
						prop[null] = Converter.Convert( split[1].Trim(), prop.Type );
					}
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

			var files = Registry.GroupBy( e => e.Value.File );
			foreach ( var file in files )
			{
				using var _ = Dev.Stopwatch( $"Saving Cookies [{file.Key}]" );

				var serialized = new StringBuilder();

				foreach ( var group in file.GroupBy( e => e.Value.Property.Group ) )
				{
					// Grouping / Sections
					if ( serialized.Length != 0 )
					{
						serialized.AppendLine();
					}

					serialized.AppendLine( $"[{group.Key}]" );

					foreach ( var (key, reference) in group )
					{
						serialized.AppendLine( $"{key} = {reference.Property[null]}" );
					}
				}

				Files.Save( "serializer.string", serialized.ToString(), file.Key );
			}

			Callback.Run( "cookies.saved" );
		}

		//
		// Structs
		//

		public readonly struct Reference
		{
			public Reference( string file, Property property )
			{
				File = file;
				Property = property;
			}

			public string File { get; }
			public Property Property { get; }
		}
	}
}
