using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	/// <summary> Espionage.Engines string based Reflection System </summary>
	[Serializable] // Instance Serialization
	public sealed partial class Library
	{
		private class internal_ComponentDatabase : IDatabase<IComponent>
		{
			public IEnumerable<IComponent> All => _components;

			public internal_ComponentDatabase( Library library )
			{
				_target = library;
			}

			private Library _target;
			private List<IComponent> _components = new List<IComponent>();

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

		public string Name;
		public string Title;
		public string Help;

		// Components

		public IDatabase<IComponent> Components;

		// Owner

		[NonSerialized]
		public Type Class;

		// GUID

		[NonSerialized]
		public Guid Id;
	}
}
