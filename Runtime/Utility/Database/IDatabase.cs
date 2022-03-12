using System.Collections.Generic;

namespace Espionage.Engine
{
	/// <summary>Generic Database, Useful for Extension Methods</summary>
	/// <typeparam name="T"> Value </typeparam>
	public interface IDatabase<T>
	{
		IEnumerable<T> All { get; }
		int Count { get; }

		void Add( T item );
		bool Contains( T item );
		void Remove( T item );
		void Clear();

		// Utility

		void Merge( IEnumerable<T> original )
		{
			foreach ( var item in original )
			{
				Add( item );
			}
		}
	}

	/// <summary>Generic Database, Useful for Extension Methods</summary>
	/// <typeparam name="TValue"> Value </typeparam>
	/// <typeparam name="TKey">Accessor Key</typeparam>
	public interface IDatabase<TValue, in TKey> : IDatabase<TValue>
	{
		TValue this[ TKey key ] { get; }
	}

	/// <summary>Generic Database, Useful for Extension Methods</summary>
	/// <typeparam name="TValue"> Value </typeparam>
	/// <typeparam name="TKey1">Accessor Key</typeparam>
	/// <typeparam name="TKey2">Accessor Key</typeparam>
	public interface IDatabase<TValue, in TKey1, in TKey2> : IDatabase<TValue, TKey1>
	{
		TValue this[ TKey2 key ] { get; }
	}

	/// <summary>Generic Database, Useful for Extension Methods</summary>
	/// <typeparam name="TValue"> Value </typeparam>
	/// <typeparam name="TKey1">Accessor Key</typeparam>
	/// <typeparam name="TKey2">Accessor Key</typeparam>
	/// <typeparam name="TKey3">Accessor Key</typeparam>
	public interface IDatabase<TValue, in TKey1, in TKey2, in TKey3> : IDatabase<TValue, TKey1, TKey2>
	{
		TValue this[ TKey3 key ] { get; }
	}
}
