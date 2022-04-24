using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Resources
{
	public partial class Resource
	{
		public static Registry Registered { get; } = new();

		public class Registry : IEnumerable<Reference>
		{
			private readonly SortedList<int, Reference> _storage = new();

			public Reference this[ string key ]
			{
				get
				{
					if ( key.IsEmpty() )
					{
						return null;
					}

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

			public Reference Fill( string path )
			{
				var hash = path.Hash();

				if ( Registered[hash] != null )
				{
					return Registered[hash];
				}

				var instance = new Reference( path );

				_storage.Add( instance.Identifier, instance );
				return instance;
			}

			public void Add( IResource resource )
			{
				// Store it in Database
				Assert.IsFalse( _storage.ContainsKey( resource.Identifier ) );
				_storage[resource.Identifier].Resource = resource;
			}

			public void Remove( Reference item )
			{
				item.Resource?.Delete();
				item.Resource = null;
			}
		}
	}
}
