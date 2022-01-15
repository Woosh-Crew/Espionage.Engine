using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	public partial class Library
	{
		private static IDatabase<Library> _database;

		private class internal_Database : IDatabase<Library>
		{
			private Dictionary<string, Library> records = new Dictionary<string, Library>();
			public IEnumerable<Library> All => records.Values;

			public void Add( Library item )
			{
				// Check if we already contain that Id, this will fuckup networking
				if ( records.Any( e => e.Value.Id == item.Id ) )
					throw new Exception( $"Library cache already contains GUID: {item.Id}" );

				records.Add( item.Name, item );
			}

			public void Clear()
			{
				records.Clear();
			}

			public bool Contains( Library item )
			{
				return records.ContainsKey( item.Name );
			}

			public void Remove( Library item )
			{
				records.Remove( item.Name );
			}

			public void Replace( Library oldItem, Library newItem )
			{
				if ( !Contains( oldItem ) )
				{
					Debugging.Log.Warning( $"Library doesnt contain item {oldItem}" );
					return;
				}

				if ( oldItem.Name != newItem.Name )
				{
					Debugging.Log.Warning( $"Cannot replace {oldItem.Title} with {newItem.Title}, because the name isn't the same." );
					return;
				}

				records[oldItem.Name] = newItem;
			}

			public string Serialize()
			{
				var json = UnityEngine.JsonUtility.ToJson( this, true );
				return json;
			}
		}
	}
}
