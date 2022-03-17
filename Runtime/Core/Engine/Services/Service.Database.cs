using System.Collections.Generic;
using System.Linq;

namespace Espionage.Engine.Services
{
	public partial class Service
	{
		public class Database : IDatabase<IService>
		{
			public IEnumerable<IService> All => _services;
			public int Count => _services.Count;
			
			private readonly List<IService> _services = new();

			public Database()
			{
				foreach ( var service in Library.Database.GetAll<IService>() )
				{
					if ( !service.Class.IsAbstract )
					{
						Add( Library.Database.Create<IService>( service.Class ) );
					}
				}
			}

			//
			// Engine
			//

			internal void Ready()
			{
				foreach ( var service in _services )
				{
					service.OnReady();
				}
			}

			internal void Update()
			{
				for ( var i = 0; i < Count; i++ )
				{
					_services[i].OnUpdate();
				}
			}

			internal void PostUpdate()
			{
				for ( var i = 0; i < Count; i++ )
				{
					_services[i].OnPostUpdate();
				}
			}

			internal void Shutdown()
			{
				for ( var i = 0; i < Count; i++ )
				{
					_services[i].OnShutdown();
				}
			}

			//
			// API
			//

			public void Add( IService item )
			{
				_services.Add( item );
			}

			public bool Contains( IService item )
			{
				return _services.Contains( item );
			}

			public void Remove( IService item )
			{
				_services.Remove( item );
				item.Dispose();
			}

			public void Clear()
			{
				foreach ( var service in _services )
				{
					service.Dispose();
				}

				_services.Clear();
			}

			public T Get<T>() where T : class, IService
			{
				return All.FirstOrDefault( e => e is T ) as T;
			}

			public bool Has<T>() where T : class, IService
			{
				return All.OfType<T>().Any();
			}
		}
	}
}
