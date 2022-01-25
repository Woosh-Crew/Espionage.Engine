using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public static class Preferences
	{
		public static IDatabase<string, Item> Database { get; set; }

		public class Item
		{
			public string Name { get; set; }
			public string Title { get; set; }
			public string Tooltip { get; set; }
		}

		private class InternalDatabase : IDatabase<Item, string>
		{
			public IEnumerable<Item> All => _records.Values;
			private readonly Dictionary<string, Item> _records = new();

			public Item this[ string key ] => _records[key];

			public void Add( Item item )
			{
				_records.Add( item.Name!, item );
			}

			public void Clear()
			{
				_records.Clear();
			}

			public bool Contains( Item item )
			{
				return _records.ContainsKey( item.Name );
			}

			public void Remove( Item item )
			{
				_records.Remove( item.Name );
			}
		}
	}
}
