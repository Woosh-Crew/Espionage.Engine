using System.Collections.Generic;

namespace Espionage.Engine
{
	public interface IDatabase<T>
	{
		IEnumerable<T> All { get; }

		void Add( T item );
		bool Contains( T item );
		void Remove( T item );
		void Replace( T oldItem, T newItem ) { }
		void Clear();

		string Serialize() { return null; }
		List<T> Deserialize( string path ) { return null; }
	}
}
