using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Components
{
	public class Components<T> : IDatabase<IComponent<T>, int> where T : class
	{
		public IEnumerable<IComponent<T>> All => _components;
		public IComponent<T> this[ int key ] => _components[key];

		public Components( T item )
		{
			_target = item;
		}

		private readonly T _target;
		private readonly List<IComponent<T>> _components = new();

		public void Add( IComponent<T> item )
		{
			if ( !item.CanAttach( _target ) )
			{
				return;
			}

			_components.Add( item );
			item.OnAttached( _target );
		}

		public bool Contains( IComponent<T> item )
		{
			return _components.Contains( item );
		}

		public void Remove( IComponent<T> item )
		{
			_components.Remove( item );
			item.OnDetached( _target );
		}

		public void Clear()
		{
			for ( var i = 0; i < _components.Count; i++ )
			{
				Remove( _components[i] );
			}

			_components.Clear();
		}

		public int Count => _components.Count;

		//
		// Accessors
		//

		public TComp Get<TComp>()
		{
			return (TComp)All.FirstOrDefault( e => e is TComp );
		}

		public TComp GetOrCreate<TComp>( Func<TComp> creation )
		{
			if ( TryGet<TComp>( out var comp ) )
			{
				return comp;
			}

			return creation.Invoke();
		}

		public IEnumerable<TComp> GetAll<TComp>()
		{
			return All.OfType<TComp>();
		}

		public bool TryGet<TComp>( out TComp output )
		{
			output = Get<TComp>();
			return output != null;
		}

		public bool Has<TComp>()
		{
			return All.OfType<TComp>().Any();
		}
	}
}
