using System;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Components
{
	public class Components<T> : IDatabase<IComponent<T>, int> where T : class
	{
		public IEnumerable<IComponent<T>> All => _components;
		public int Count => _components.Count;

		public event Action<IComponent<T>> OnAdded;
		public event Action<IComponent<T>> OnRemove;

		public IComponent<T> this[ int key ] => _components[key];

		//
		// Instance
		//

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

			OnAdded?.Invoke( item );
		}

		public bool Contains( IComponent<T> item )
		{
			return _components.Contains( item );
		}

		public void Remove( IComponent<T> item )
		{
			_components.Remove( item );
			item.OnDetached();

			OnRemove?.Invoke( item );
		}

		public void Clear()
		{
			for ( var i = 0; i < _components.Count; i++ )
			{
				Remove( _components[i] );
			}

			_components.Clear();
		}

		//
		// Accessors
		//

		public TComp Get<TComp>()
		{
			return (TComp)All.FirstOrDefault( e => e is TComp );
		}

		public TComp GetOrCreate<TComp>( Func<TComp> creation )
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

		public void Replace<TComp>( IComponent<T> old, Func<TComp> creation )
		{
			Replace( old, Create( creation ) as IComponent<T> );
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
