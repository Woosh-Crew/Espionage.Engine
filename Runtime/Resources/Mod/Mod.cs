using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Components;
using Espionage.Engine.Resources;

namespace Espionage.Engine
{
	[Library( "res.mod" ), Group( "Mods" ), Path( "mods", "assets://Mods" )]
	public class Mod : ILibrary, IResource
	{
		public Library ClassInfo { get; }
		public Components<Mod> Components { get; }

		// Meta

		public string Title => Components.TryGet<Meta>( out var meta ) ? meta.Title : Identifier.ToTitleCase();
		public string Identifier { get; }
		public string Path { get; }

		public Mod( string name, string path )
		{
			ClassInfo = Library.Register( this );

			Identifier = name;
			Path = path;
			Components = new( this );

			Database.Add( this );
			Files.Pathing.Add( name, path );

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

		public void Delete()
		{
			Library.Unregister( this );
			Database.Remove( this );
		}

		public bool Exists( string path, out string full )
		{
			full = Files.Pathing.Absolute( "assets://" ) + Files.Pathing.Relative( "assets://", Path + "/" + Files.Pathing.Relative( "assets://", path ) );
			return Files.Pathing.Exists( full );

		}

		//
		// Database
		//

		public static IDatabase<Mod, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<Mod, string>
		{
			// IDatabase

			public int Count => _records.Count;
			public Mod this[ string key ] => _records.ContainsKey( key ) ? _records[key] : null;

			// Instance

			private readonly Dictionary<string, Mod> _records = new( StringComparer.CurrentCultureIgnoreCase );

			// Enumerator

			public IEnumerator<Mod> GetEnumerator()
			{
				// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
				return Count == 0 ? Enumerable.Empty<Mod>().GetEnumerator() : _records.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			// API

			public void Add( Mod item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Identifier! ) )
				{
					Debugging.Log.Warning( $"Replacing Map [{item.Identifier}]" );
					_records[item.Identifier] = item;
				}
				else
				{
					_records.Add( item.Identifier!, item );
				}
			}

			public void Clear()
			{
				_records.Clear();
			}

			public bool Contains( Mod item )
			{
				return _records.ContainsKey( item.Identifier );
			}

			public void Remove( Mod item )
			{
				_records.Remove( item.Identifier );
			}
		}
	}
}
