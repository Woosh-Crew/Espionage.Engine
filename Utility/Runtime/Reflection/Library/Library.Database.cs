using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine
{
	public partial class Library
	{
		/// <summary>
		/// Database for Library Records. Allows the access of all records.
		/// Use extension methods to add functionality to database access.
		/// </summary>
		public static IDatabase<Library, string, Type> Database { get; private set; }

		private class InternalDatabase : IDatabase<Library, string, Type>
		{
			public IEnumerable<Library> All => _records.Values;
			private readonly Dictionary<string, Library> _records = new();

			public Library this[ string key ] => _records[key];
			public Library this[ Type key ] => this.Get( key );


			public void Add( Library item )
			{
				if ( string.IsNullOrEmpty( item.Title ) )
				{
					// Do space before uppercase char 
					var type = item.Class;

					// This is so aids...
					if ( type.IsGenericType )
					{
						item.Title = (type.IsInterface ? type.Name : string.Concat( type.Name!.Select( x => char.IsUpper( x ) ? " " + x : x.ToString() ) ).TrimStart( ' ' )).Split( '`' )[0] + $"<{type.GetGenericArguments()[0]?.Name}>";
					}
					else
					{
						item.Title = type.IsInterface ? type.Name : string.Concat( type.Name!.Select( x => char.IsUpper( x ) ? " " + x : x.ToString() ) ).TrimStart( ' ' );
					}
				}

				if ( string.IsNullOrEmpty( item.Group ) )
				{
					item.Group = item.Class.Namespace;
				}

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

			public bool Contains( Library item )
			{
				return _records.ContainsKey( item.Name );
			}

			public void Remove( Library item )
			{
				_records.Remove( item.Name );
			}
		}
	}
}
