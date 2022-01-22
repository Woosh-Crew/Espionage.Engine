using System.Collections.Generic;

namespace Espionage.Engine.Assets
{
	public sealed partial class Map
	{
		public static IDatabase<Map, string> Database { get; private set; }
		
		private class InternalDatabase : IDatabase<Map, string>
		{
			public IEnumerable<Map> All => _records.Values;
			private readonly Dictionary<string, Map> _records = new();

			public Map this[ string key ] => _records[key];


			public void Add( Map item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Path! ) )
				{
					_records[item.Path] = item;
				}
				else
				{
					_records.Add( item.Path!, item );
				}
			}

			public void Clear()
			{
				_records.Clear();
			}

			public bool Contains( Map item )
			{
				return _records.ContainsKey( item.Path );
			}

			public void Remove( Map item )
			{
				_records.Remove( item.Path );
			}
		}
	}
}
