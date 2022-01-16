using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Espionage.Engine
{
	/// <summary> Espionage.Engines string based Reflection System </summary>
	[Serializable]
	public sealed partial class Library
	{
		private class InternalComponentDatabase : IDatabase<IComponent>
		{
			public IEnumerable<IComponent> All => _components;

			public InternalComponentDatabase( Library library )
			{
				_target = library;
			}

			private readonly Library _target;
			private readonly List<IComponent> _components = new();

			public void Add( IComponent item )
			{
				item.Library = _target;
				_components.Add( item );
				item.OnAttached();
			}

			public void Clear()
			{
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
				item.Library = null;
			}
		}

		public Library() { }

		public Library( Type type )
		{
			Class = type;
			Name = type.FullName;
		}

		// Meta
		private string _name;

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				Id = GenerateID( _name );
			}
		}

		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }
		public bool Spawnable { get; set; }

		// Components
		public IDatabase<IComponent> Components { get; private set; }

		// Owner & Identification
		public Type Class { get; set; }
		public Guid Id { get; private set; }
	}
}
