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
		private class internal_ComponentDatabase : IDatabase<Component>
		{
			public IEnumerable<Component> All => _components;

			public internal_ComponentDatabase( Library library )
			{
				_target = library;
			}

			private Library _target;
			private List<Component> _components = new List<Component>();

			public void Add( Component item )
			{
				item.Library = _target;
				_components.Add( item );
				item.OnAttached();
			}

			public void Clear()
			{
				_components.Clear();
			}

			public bool Contains( Component item )
			{
				return _components.Contains( item );
			}

			public void Remove( Component item )
			{
				_components.Remove( item );
			}
		}

		// Meta

		public string Name;
		public string Title;
		public string Help;
		public string Icon;

		// Components

		public IDatabase<Component> Components;

		// Owner

		[NonSerialized]
		public Type Class;

		// GUID

		[NonSerialized]
		public Guid Id;
	}
}
