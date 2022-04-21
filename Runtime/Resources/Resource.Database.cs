using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Resources
{
	public partial class Resource
	{
		public static Registry Registered { get; } = new();

		public class Registry : IEnumerable<Registry.Reference>
		{
			private readonly SortedList<int, Reference> _storage = new();

			public Reference this[ string key ]
			{
				get
				{
					var hash = key.Hash();
					return _storage.ContainsKey( hash ) ? _storage[hash] : null;
				}
			}

			public Reference this[ int key ] => _storage.ContainsKey( key ) ? _storage[key] : null;
			public int Count => _storage.Count;

			// Enumerator

			public IEnumerator<Reference> GetEnumerator()
			{
				// This is bit hacky, but its a facade API! I love facade APIs!
				return Count == 0 ? Enumerable.Empty<Reference>().GetEnumerator() : _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			//
			// API
			//

			public void Fill( string path )
			{
				var instance = new Reference( path );
				_storage.Add( instance.Identifier, instance );
			}

			public void Add( IResource item )
			{
				// Store it in Database
				if ( _storage.ContainsKey( item.Identifier ) )
				{
					_storage[item.Identifier].Resource = item;
				}
				else
				{
					Debugging.Log.Warning( $"Adding new resource [{item.Identifier}]" );
					_storage.Add( item.Identifier, new( item.Identifier ) );
				}
			}

			public void Remove( IResource item )
			{
				_storage[item.Identifier].Resource = null;
				item.Delete();
			}

			// 
			// Data
			//

			public class Reference
			{
				public IResource Resource { get; set; }

				public Reference( string path )
				{
					Path = path;
					Identifier = path.Hash();
				}

				public Reference( int hash )
				{
					Path = null;
					Identifier = hash;
				}

				~Reference()
				{
					Resource = null;
				}

				public string Path { get; }
				public int Identifier { get; }

				public override string ToString()
				{
					return $"loaded:[{Resource != null}] path:[{Path}]";
				}
			}
		}
	}
}
