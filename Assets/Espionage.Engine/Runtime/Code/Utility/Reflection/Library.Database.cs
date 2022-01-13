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


		[Serializable] // Database Serialization
		private class internal_Database : IDatabase<Library>
		{
#if UNITY_STANDALONE || UNITY_EDITOR
			[UnityEngine.SerializeField]
#endif
			private List<Library> records = new List<Library>();
			public IEnumerable<Library> All => records;

			public void Add( Library item )
			{
				// Check if we already have that name
				if ( records.Any( e => e.Name == item.Name ) )
					throw new Exception( $"Library cache already contains key: {item.Name}" );

				// Check if we already contain that Id, this will fuckup networking
				if ( records.Any( e => e.Id == item.Id ) )
					throw new Exception( $"Library cache already contains GUID: {item.Id}" );

				records.Add( item );
			}

			public void Clear()
			{
				records.Clear();
			}

			public bool Contains( Library item )
			{
				return records.Contains( item );
			}

			public void Remove( Library item )
			{
				records.Remove( item );
			}

			public void Replace( Library oldItem, Library newItem )
			{
				if ( !Contains( oldItem ) )
				{
					Debugging.Log.Warning( $"Library doesnt contain item {oldItem}" );
					return;
				}

				var index = records.IndexOf( oldItem );
				records[index] = newItem;
			}

			public string Serialize()
			{
				var json = UnityEngine.JsonUtility.ToJson( this, true );
				return json;
			}
		}
	}
}
