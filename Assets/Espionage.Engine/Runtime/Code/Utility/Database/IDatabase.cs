using System.Collections.Generic;

namespace Espionage.Engine
{
	public interface IDatabase<T>
	{
		IEnumerable<T> All { get; }

		void Add( T item );
		void Contains( T item );
		void Remove( T item );
		void Clear();
	}
}
