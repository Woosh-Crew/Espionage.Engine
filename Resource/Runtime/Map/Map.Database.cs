using System.Collections.Generic;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		/// <summary>
		/// A reference to all the maps that have already been found or loaded.
		/// </summary>
		public static IDatabase<Map, string> Database { get; private set; }

		private class InternalDatabase : IDatabase<Map, string>
		{
			public IEnumerable<Map> All => _records.Values;
			private readonly Dictionary<string, Map> _records = new();

			public Map this[ string key ] => _records.ContainsKey( key ) ? _records[key] : null;

			public void Add( Map item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Identifier! ) )
				{
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

			public bool Contains( Map item )
			{
				return _records.ContainsKey( item.Identifier );
			}

			public void Remove( Map item )
			{
				_records.Remove( item.Identifier );
			}
		}
	}
}
