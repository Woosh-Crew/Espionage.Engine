using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Components
{
	public class Components<T> : IDatabase<IComponent<T>, int> where T : class
	{
		public IEnumerable<IComponent<T>> All => _storage;
		public int Count => _storage.Count;

		public event Action<IComponent<T>> OnAdded;
		public event Action<IComponent<T>> OnRemove;

		public IComponent<T> this[ int key ] => _storage[key];

		//
		// Instance
		//

		public Components( T item )
		{
			_owner = item;
		}

		private readonly T _owner;
		private readonly List<IComponent<T>> _storage = new();

		public void Add( IComponent<T> item )
		{
			if ( !item.CanAttach( _owner ) )
			{
				return;
			}

			_storage.Add( item );
			item.OnAttached( _owner );

			OnAdded?.Invoke( item );
		}

		public bool Contains( IComponent<T> item )
		{
			return _storage.Contains( item );
		}

		public void Remove( IComponent<T> item )
		{
			_storage.Remove( item );
			item.OnDetached();

			OnRemove?.Invoke( item );
		}

		public void Clear()
		{
			for ( var i = 0; i < _storage.Count; i++ )
			{
				Remove( _storage[i] );
			}

			_storage.Clear();
		}

		//
		// Accessors
		//

		public TComp Get<TComp>() where TComp : class
		{
			return All.FirstOrDefault( e => e is TComp ) as TComp;
		}

		public IComponent<T> Get( Type type )
		{
			return All.FirstOrDefault( e => e.GetType() == type );
		}

		public TComp GetOrCreate<TComp>() where TComp : class, new()
		{
			return TryGet<TComp>( out var comp ) ? comp : new();
		}

		public TComp GetOrCreate<TComp>( Func<TComp> creation ) where TComp : class
		{
			return TryGet<TComp>( out var comp ) ? comp : Create( creation );
		}

		public TComp Create<TComp>( Func<TComp> creation )
		{
			var newComp = creation.Invoke();
			Add( newComp as IComponent<T> );
			return newComp;
		}

		public void Replace( IComponent<T> old, IComponent<T> newComp )
		{
			if ( old == null || newComp == null )
			{
				Dev.Log.Error( $"Components aren't valid" );
				return;
			}

			if ( !Contains( old ) )
			{
				Dev.Log.Error( $"Components doesnt contain {old}" );
				return;
			}

			Remove( old );
			Add( newComp );
		}

		public void Replace<TComp>( IComponent<T> old, Func<TComp> creation )
		{
			Replace( old, Create( creation ) as IComponent<T> );
		}

		public IEnumerable<TComp> GetAll<TComp>()
		{
			return All.OfType<TComp>();
		}

		public bool TryGet<TComp>( out TComp output ) where TComp : class
		{
			output = Get<TComp>();
			return output != null;
		}

		public bool Has<TComp>()
		{
			return All.OfType<TComp>().Any();
		}

		public bool Has( Type type )
		{
			return All.Any( e => e.GetType() == type );
		}
	}
}
