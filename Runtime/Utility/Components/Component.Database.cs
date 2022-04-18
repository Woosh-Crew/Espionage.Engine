﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Components
{
	public class Components<T> : IDatabase<IComponent<T>, int, Type> where T : class
	{
		// IDatabase

		public int Count => _storage.Count;

		public IComponent<T> this[ int key ] => _storage[key];
		public IComponent<T> this[ Type key ] => Get( key );

		// Instance

		public Components( T item )
		{
			_owner = item;
		}

		// Database

		private readonly T _owner;
		private readonly List<IComponent<T>> _storage = new();

		// Enumerator

		public IEnumerator<IComponent<T>> GetEnumerator()
		{
			// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
			return Count == 0 ? Enumerable.Empty<IComponent<T>>().GetEnumerator() : _storage.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		// Controllers

		public void Add( IComponent<T> item )
		{
			if ( !item.CanAttach( _owner ) )
			{
				return;
			}

			// Replace if its a Singleton
			if ( item is ILibrary lib && lib.ClassInfo.Components.Has<SingletonAttribute>() && TryGet( item.GetType(), out var comp ) )
			{
				Debugging.Log.Warning( $"Replacing Component [{lib.ClassInfo.Name}]. Was Singleton" );
				Replace( comp, item );
				return;
			}

			_storage.Add( item );
			item.OnAttached( _owner );
		}

		public bool Contains( IComponent<T> item )
		{
			return _storage.Contains( item );
		}

		public void Remove( IComponent<T> item )
		{
			_storage.Remove( item ); 
			item.OnDetached();
		}

		public void Clear()
		{
			for ( var i = 0; i < _storage.Count; i++ )
			{
				Remove( _storage[i] );
			}

			_storage.Clear();
		}

		// Utility

		public TComp Get<TComp>() where TComp : class
		{
			var index = 0;
			while ( index <= Count - 1 )
			{
				var item = _storage[index];

				if ( item is TComp comp )
				{
					return comp;
				}

				index++;
			}

			return null;
		}

		public IComponent<T> Get( Type type )
		{
			var index = 0;
			while ( index <= Count - 1 )
			{
				var item = _storage[index];

				if ( item.GetType() == type )
				{
					return item;
				}

				index++;
			}

			return null;
		}

		// Get or Create

		public TComp GetOrCreate<TComp>() where TComp : class, IComponent<T>, new()
		{
			return TryGet<TComp>( out var comp ) ? comp : Create<TComp>();
		}

		public TComp GetOrCreate<TComp>( Func<TComp> creation ) where TComp : class
		{
			return TryGet<TComp>( out var comp ) ? comp : Create( creation );
		}

		// Create

		public TComp Create<TComp>() where TComp : class, IComponent<T>, new()
		{
			var newComp = new TComp();
			Add( newComp );
			return newComp;
		}

		public TComp Create<TComp>( Func<TComp> creation )
		{
			var newComp = creation.Invoke();
			Add( newComp as IComponent<T> );
			return newComp;
		}

		// Build

		public Components<T> Build<TComp>() where TComp : class, IComponent<T>, new()
		{
			Create<TComp>();
			return this;
		}

		public Components<T> Build<TComp>( out TComp comp ) where TComp : class, IComponent<T>, new()
		{
			comp = Create<TComp>();
			return this;
		}

		// Replace

		public void Replace( IComponent<T> old, IComponent<T> newComp )
		{
			if ( old == null || newComp == null )
			{
				Debugging.Log.Error( $"Components aren't valid" );
				return;
			}

			if ( !Contains( old ) )
			{
				Debugging.Log.Error( $"Components doesnt contain {old}" );
				return;
			}

			Remove( old );
			Add( newComp );
		}

		// All

		public IEnumerable<TComp> GetAll<TComp>()
		{
			return _storage.OfType<TComp>();
		}

		// Try Get

		public bool TryGet<TComp>( out TComp output ) where TComp : class
		{
			output = Get<TComp>();
			return output != null;
		}

		public bool TryGet( Type type, out IComponent<T> output )
		{
			output = Get( type );
			return output != null;
		}

		// Has

		public bool Has<TComp>()
		{
			return Has( typeof( TComp ) );
		}

		public bool Has( Type type )
		{
			var index = 0;
			while ( index <= Count - 1 )
			{
				var item = _storage[index];

				if ( item.GetType() == type || item.GetType().IsSubclassOf( type ) )
				{
					return true;
				}

				index++;
			}

			return false;
		}
	}
}
