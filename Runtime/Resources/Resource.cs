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
	}

}
