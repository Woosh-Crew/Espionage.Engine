using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;
using Random = System.Random;

namespace Espionage.Engine
{
	public partial class Library
	{
		/// <summary> Database for library records </summary>
		public static IDatabase<Library, string, Type> Database { get; private set; }

		private class InternalDatabase : IDatabase<Library, string, Type>
		{
			public IEnumerable<Library> All => _records.Values;
			private readonly Dictionary<string, Library> _records = new();

			public Library this[ string key ] => _records[key];
			public Library this[ Type key ] => this.Get( key );


			public void Add( Library item )
			{
				if ( item.Class is null )
				{
					throw new Exception( $"Library doesn't have an owning class: {item.Name}" );
				}

				if ( string.IsNullOrEmpty( item.Name ) )
				{
					item.Name = item.Class.FullName;
				}

				if ( string.IsNullOrEmpty( item.Title ) )
				{
					item.Title = item.Class.Name;
				}

				if ( string.IsNullOrEmpty( item.Group ) )
				{
					item.Group = item.Class.Namespace;
				}

				_records.Add( item.Name!, item );
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
