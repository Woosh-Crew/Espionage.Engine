using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Espionage.Engine
{
	/// <summary> Espionage.Engines string based Reflection System </summary>
	[Serializable] // Instance Serialization
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
			private readonly List<IComponent> _components = new List<IComponent>();

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

		// Meta
		public string name;
		public string title;
		public string help;
		public bool spawnable;

		// Components
		public IDatabase<IComponent> Components { get; private set; }

		// Owner
		[NonSerialized]
		public Type Class;

		// GUID
		[NonSerialized]
		public Guid Id;
	}
}
