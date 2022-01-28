using System.Collections.Generic;

namespace Espionage.Engine.Components
{
	public class ComponentDatabase<T> : IDatabase<IComponent<T>> where T : class
	{
		public IEnumerable<IComponent<T>> All => _components;

		public ComponentDatabase( T item )
		{
			_target = item;
		}

		private readonly T _target;
		private readonly List<IComponent<T>> _components = new();

		public void Add( IComponent<T> item )
		{
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
			foreach ( var item in _components )
			{
				Remove( item );
			}

			_components.Clear();
		}
	}
}
