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
		private class InternalDatabase : IDatabase<Library>
		{
			public IEnumerable<Library> All => _records.Values;
			private readonly Dictionary<string, Library> _records = new();

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
					item.Title = item.Name;
				}

				if ( string.IsNullOrEmpty( item.Group ) )
				{
					item.Group = item.Class.Namespace;
				}

				// Check if we already contain that Id, this will fuck up networking
				if ( _records.Any( e => e.Value.Id == item.Id ) )
				{
					throw new Exception( $"Library cache already contains GUID: {item.Id}" );
				}

				_records.Add( item.Name ?? throw new InvalidOperationException(), item );
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

				_records[oldItem.Name] = newItem;
			}

			public string Serialize()
			{
				var json = UnityEngine.JsonUtility.ToJson( this, true );
				return json;
			}
		}
	}
}
