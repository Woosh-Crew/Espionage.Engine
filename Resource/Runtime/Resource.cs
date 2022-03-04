using System;
using System.Collections.Generic;

namespace Espionage.Engine.Resources
{
	[Group( "Resources" )]
	public abstract class Resource : IResource, IDisposable, ILibrary
	{
		public Library ClassInfo { get; }

		public Resource()
		{
			ClassInfo = Library.Register( this );
		}

		~Resource()
		{
			Library.Unregister( this );
		}

		// Resource

		public abstract string Identifier { get; }
		public virtual bool IsLoading { get; protected set; }

		void IResource.Load( Action onLoad )
		{
			Database.Add( this );
			OnLoad( onLoad );
		}

		protected virtual void OnLoad( Action onLoad ) { }

		void IResource.Unload( Action onUnload )
		{
			Database.Remove( this );
			OnUnload( onUnload );
		}

		protected virtual void OnUnload( Action onUnload ) { }

		public void Dispose()
		{
			(this as IResource).Unload();
		}

		public interface IProvider<T, out TOutput> where T : IResource
		{
			TOutput Output { get; }
			float Progress { get; }

			string Identifier { get; }
			bool IsLoading { get; }

			void Load( Action onLoad = null );
			void Unload( Action onUnload = null );
		}

		//
		// Database
		//

		///	<summary> A reference to all resources that are loaded. </summary>
		public static IDatabase<Resource, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<Resource, string>
		{
			public IEnumerable<Resource> All => _records.Values;
			private readonly Dictionary<string, Resource> _records = new();

			public Resource this[ string key ] => _records.ContainsKey( key ) ? _records[key] : null;

			public void Add( Resource item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Identifier! ) )
				{
					_records[item.Identifier] = item;
					Debugging.Log.Warning( $"For some reason we're replacing a resource? [{item.Identifier}]" );
				}
				else
				{
					_records.Add( item.Identifier!, item );
				}
			}

			public void Clear()
			{
				_records.Clear();
			}

			public bool Contains( Resource item )
			{
				return _records.ContainsKey( item.Identifier );
			}

			public void Remove( Resource item )
			{
				_records.Remove( item.Identifier );
			}

			public int Length => _records.Count;
		}
	}
}
