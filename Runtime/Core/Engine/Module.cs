using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine
{
	[Group( "Modules" ), Singleton]
	public abstract class Module : ILibrary
	{
		public Library ClassInfo { get; }

		public Module()
		{
			ClassInfo = Library.Register( this );
		}

		// State
		protected virtual bool OnRegister() { return true; }
		protected virtual void OnReady() { }
		protected virtual void OnShutdown() { }

		// Update
		protected virtual void OnUpdate() { }

		// Registry
		public class Container : IEnumerable<Module>
		{
			private Dictionary<int, Module> _storage { get; } = new();

			public Container()
			{
				foreach ( var library in Library.Database.GetAll<Module>() )
				{
					Create( library );
				}

				_storage = _storage.OrderBy( e => e.Value.ClassInfo.Components.Get<OrderAttribute>()?.Order ?? 5 ).ToDictionary( e => e.Key, e => e.Value );
			}

			public Module this[ Library lib ] => _storage.ContainsKey( lib.Id ) ? _storage[lib.Id] : null;

			public T Get<T>() where T : Module
			{
				return (T)this[typeof( T )];
			}

			public Module Create( Library library )
			{
				if ( library.Info.IsAbstract )
				{
					return null;
				}

				var module = Library.Create<Module>( library.Info );

				if ( module.OnRegister() )
				{
					_storage.Add( library.Id, module );
					return module;
				}

				// Delete, couldn't register
				Library.Unregister( module );
				return null;
			}

			// Internal

			internal void Ready()
			{
				foreach ( var module in _storage.Values )
				{
					module.OnReady();
				}
			}

			internal void Shutdown()
			{
				foreach ( var module in _storage.Values )
				{
					module.OnShutdown();
				}
			}

			internal void Frame()
			{
				foreach ( var module in _storage.Values )
				{
					module.OnUpdate();
				}
			}

			// Enumerator

			public IEnumerator<Module> GetEnumerator()
			{
				return _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

	}
}
