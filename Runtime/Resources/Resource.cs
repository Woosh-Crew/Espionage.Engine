using System;
using System.Collections.Generic;

namespace Espionage.Engine.Resources
{
	public abstract class Resource<T> : ILibrary, IDisposable, IResource where T : class
	{
		public Library ClassInfo { get; }

		public Resource()
		{
			ClassInfo = Library.Register( this );
		}

		public string Identifier { get; protected set; }

		protected abstract void Load( Action<T> onLoad );
		protected abstract void Unload();

		public void Dispose()
		{
			Unload();
		}

		//
		// Database
		//

		public static IDatabase<Resource<T>, string> Database { get; } = new ResourceDatabase<T>();

		private class ResourceDatabase<TResource> : IDatabase<Resource<TResource>, string> where TResource : class
		{
			public IEnumerable<Resource<TResource>> All { get; }
			public int Count { get; }

			public Resource<TResource> this[ string key ] => throw new NotImplementedException();

			public void Add( Resource<TResource> item ) { }

			public bool Contains( Resource<TResource> item ) { return false; }

			public void Remove( Resource<TResource> item ) { }

			public void Clear() { }
		}
	}

}
