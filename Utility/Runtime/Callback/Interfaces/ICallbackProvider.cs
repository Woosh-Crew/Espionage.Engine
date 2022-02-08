using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Espionage.Engine.Internal.Callbacks
{
	public interface ICallbackProvider : IDisposable
	{
		void Run( string name );
		object[] Run( string name, params object[] args );

		void Register( object item );
		void Unregister( object item );
	}
}
