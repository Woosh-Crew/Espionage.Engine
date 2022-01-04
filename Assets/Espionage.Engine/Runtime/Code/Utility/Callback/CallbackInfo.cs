// Attribute based event callback system

using System;
using System.Reflection;

namespace Espionage.Engine.Internal.Callbacks
{
	internal struct CallbackInfo
	{
		// Class
		public Type Class { get; internal set; }
		public bool IsStatic { get; internal set; }


		// Delegate
		public delegate object CallbackEvent( object target, object[] args );
		private CallbackEvent _callback;

		public object Invoke( object target = null, object[] args = null )
		{
			return _callback?.Invoke( target, args );
		}

		//
		// Builder
		//

		public CallbackInfo WithCallback( CallbackEvent callbackEvent )
		{
			_callback = callbackEvent;
			return this;
		}

		public CallbackInfo FromType( Type type )
		{
			Class = type;
			return this;
		}
	}
}
