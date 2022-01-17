using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Espionage.Engine
{
	public sealed class Property
	{
		internal Property( Library owner, PropertyInfo info )
		{
			ClassInfo = owner;
			Info = info;

			Name = info.Name;
			Title = info.Name;

			//
			// Components
			Components = new InternalComponentDatabase( this );

			// This is really expensive (6ms)...
			// Get Components attached to type
			foreach ( var item in Info.GetCustomAttributes().Where( e => e is IComponent ) )
			{
				Components.Add( item as IComponent );
			}
		}

		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }

		public Library ClassInfo { get; }
		public PropertyInfo Info { get; }

		public void SetValue( object from, object value )
		{
			Info.SetValue( from, value );
		}

		public object GetValue( object from )
		{
			return Info.GetValue( from );
		}

		//
		// Component
		//

		public IDatabase<IComponent> Components { get; private set; }

		public interface IComponent
		{
			void OnAttached( ref Property property );
			void OnDetached() { }
		}

		private class InternalComponentDatabase : IDatabase<IComponent>
		{
			public IEnumerable<IComponent> All => _components;

			public InternalComponentDatabase( Property target )
			{
				_target = target;
			}

			private Property _target;
			private readonly List<IComponent> _components = new();

			public void Add( IComponent item )
			{
				_components.Add( item );
				item.OnAttached( ref _target );
			}

			public void Clear()
			{
				foreach ( var item in _components )
				{
					Remove( item );
				}

				_components.Clear();
			}

			public bool Contains( IComponent item )
			{
				return _components.Contains( item );
			}

			public void Remove( IComponent item )
			{
				_components.Remove( item );
				item.OnDetached();
			}
		}
	}
}
