using System.Collections.Generic;

namespace Espionage.Engine.Resources
{
	public partial class Resource<T>
	{
		public static IDatabase<IResource, string> Database { get; }

		private class InternalDatabase : IDatabase<IResource, string>
		{
			public IEnumerable<IResource> All => _records.Values;
			private readonly Dictionary<string, IResource> _records = new();

			public IResource this[ string key ] => _records[key];

			public void Add( IResource item )
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

			public bool Contains( IResource item )
			{
				return _records.ContainsKey( item.Path );
			}

			public void Remove( IResource item )
			{
				_records.Remove( item.Path );
			}
		}
	}
}
