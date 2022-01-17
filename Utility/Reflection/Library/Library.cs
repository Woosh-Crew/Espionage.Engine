using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Espionage.Engine
{
	/// <summary>
	/// <para>
	/// The Library is Espionage.Engines string based Reflection System.
	/// </para>
	/// <para>
	/// Libraries are used for storing meta data on a type, this includes
	/// Title, Name, Group, Icons, etc. You can also add your own data
	/// using components.
	/// We can do a lotta cool and performant shit because of this.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed partial class Library
	{
		public Library( [NotNull] Type type )
		{
			Class = type;
			Name = type.FullName;

			//
			// Components
			Components = new InternalComponentDatabase( this );

			// This is really expensive (6ms)...
			// Get Components attached to type
			foreach ( var item in Class.GetCustomAttributes().Where( e => e is IComponent ) )
			{
				Components.Add( item as IComponent );
			}

			//
			// Get Properties
			Properties = new InternalPropertyDatabase();

			// Get all Properties (Defined by the User)
			const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
			foreach ( var propertyInfo in Class.GetProperties( flags ).Where( e => e.IsDefined( typeof( PropertyAttribute ) ) ) )
			{
				var attribute = propertyInfo.GetCustomAttribute<PropertyAttribute>();
				Properties.Add( attribute.CreateRecord( this, propertyInfo ) );
			}
		}

		// Meta
		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }
		public bool Spawnable { get; set; }

		// Owner & Identification
		public Type Class { get; set; }
		public Guid Id => GenerateID( $"{Group}/{Name}" );

		//
		// Properties
		// 

		public IDatabase<Property> Properties { get; private set; }

		private class InternalPropertyDatabase : IDatabase<Property>
		{
			private readonly Dictionary<string, Property> _all = new();
			public IEnumerable<Property> All => _all.Values;

			public void Add( Property item )
			{
				_all.Add( item.Name, item );
			}

			public bool Contains( Property item )
			{
				return _all.ContainsKey( item.Name );
			}

			public void Remove( Property item )
			{
				_all.Remove( item.Name );
			}

			public void Clear()
			{
				_all.Clear();
			}
		}

		//
		// Components
		//

		public IDatabase<IComponent> Components { get; private set; }

		private class InternalComponentDatabase : IDatabase<IComponent>
		{
			public IEnumerable<IComponent> All => _components;

			public InternalComponentDatabase( Library library )
			{
				_target = library;
			}

			private Library _target;
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
