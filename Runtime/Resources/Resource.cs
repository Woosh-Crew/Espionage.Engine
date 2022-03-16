using System;
using System.Collections.Generic;

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// A Resource is a loadable asset, that is loaded in editor or at runtime.
	/// Resources will keep references to the instances and the assets identifier.
	/// </summary>
	[Group( "Resources" )]
	public abstract class Resource<T> : IResource<T>, ILibrary
	{
		public Library ClassInfo { get; }

		protected Resource()
		{
			ClassInfo = Library.Register( this );
		}

		~Resource()
		{
			Library.Unregister( this );
		}

		//
		// Resource
		//

		/// <summary> The path / database entry to this resource. </summary>
		public abstract string Identifier { get; }

		/// <summary> Is this resource actually loaded? </summary>
		protected bool IsLoaded => Database.Contains( this );

		public virtual void Load( Action<T> loaded = null )
		{
			if ( !IsLoaded )
			{
				Database.Add( this );
			}
		}

		public virtual void Unload( Action unloaded = null )
		{
			Database.Remove( this );
		}

		//
		// Database
		//

		///	<summary> A reference to all resources that are loaded. </summary>
		public static IDatabase<IResource, string> Database { get; } = new InternalDatabase();

		private class InternalDatabase : IDatabase<IResource, string>
		{
			public IEnumerable<IResource> All => _records.Values;
			public int Count => _records.Count;

			private readonly Dictionary<string, IResource> _records = new();

			public IResource this[ string key ] => _records.ContainsKey( key ) ? _records[key] : null;

			public void Add( IResource item )
			{
				// Store it in Database
				if ( _records.ContainsKey( item.Identifier! ) )
				{
					Debugging.Log.Error( $"For some reason we're replacing a resource? [{item.Identifier}]" );
					return;
				}

				_records.Add( item.Identifier!, item );
			}

			public void Clear()
			{
				_records.Clear();
			}

			public bool Contains( IResource item )
			{
				return _records.ContainsKey( item.Identifier );
			}

			public void Remove( IResource item )
			{
				_records.Remove( item.Identifier );
			}
		}
	}
}
