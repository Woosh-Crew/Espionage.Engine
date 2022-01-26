using System.Collections.Generic;

namespace Espionage.Engine.Languages
{
	public static class Localisation
	{
		public static Language Current { get; private set; }
		public static IDatabase<Language, string> Database { get; } = new InternalDatabase();

		public static void Switch( Language language )
		{
			Current = language;
			Callback.Run( "localisation.switched" );
		}

		//
		// Database
		//

		private class InternalDatabase : IDatabase<Language, string>
		{
			public IEnumerable<Language> All => _records.Values;
			private readonly Dictionary<string, Language> _records = new();

			public Language this[ string key ] => _records.ContainsKey( key ) ? _records[key] : null;

			public void Add( Language item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Name! ) )
				{
					_records[item.Name] = item;
				}
				else
				{
					_records.Add( item.Name!, item );
				}
			}

			public void Clear()
			{
				_records.Clear();
			}

			public bool Contains( Language item )
			{
				return _records.ContainsKey( item.Name );
			}

			public void Remove( Language item )
			{
				_records.Remove( item.Name );
			}
		}
	}
}
