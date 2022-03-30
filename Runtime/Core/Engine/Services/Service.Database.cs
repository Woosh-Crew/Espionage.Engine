using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Espionage.Engine.Services
{
	public partial class Service
	{
		public sealed class Database : IDatabase<IService>
		{
			public int Count => _services.Count;

			internal Database()
			{
				foreach ( var service in Library.Database.GetAll<IService>() )
				{
					if ( !service.Info.IsAbstract )
					{
						Add( Library.Database.Create<IService>( service.Info ) );
					}

					_services = _services.OrderBy( e => e.ClassInfo.Components.Get<OrderAttribute>()?.Order ?? 5 ).ToList();
				}
			}

			private readonly List<IService> _services = new();

			// Enumerator

			public IEnumerator<IService> GetEnumerator()
			{
				// This shouldn't box. _store.GetEnumerator Does. but Enumerable.Empty shouldn't.
				return Count == 0 ? Enumerable.Empty<IService>().GetEnumerator() : _services.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}


			// Engine

			internal void Ready()
			{
				foreach ( var service in _services )
				{
					var stopwatch = Stopwatch.StartNew();
					service.OnReady();
					stopwatch.Stop();
					service.Time = stopwatch.ElapsedMilliseconds;
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

			// API

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
				return this.FirstOrDefault( e => e is T ) as T;
			}

			public bool Has<T>() where T : class, IService
			{
				return this.OfType<T>().Any();
			}
		}
	}
}
