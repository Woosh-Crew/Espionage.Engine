// Attribute based event callback system

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Espionage.Engine.Internal.Callbacks
{
	internal struct CallbackInfo
	{
		// Class
		public Type Class { get; internal set; }
		public bool IsStatic { get; internal set; }


		// Delegate
		public delegate object Action( object target, object[] args );
		private Action _callback;

		public object Invoke( object target = null, object[] args = null )
		{
			return _callback?.Invoke( target, args );
		}

		//
		// Builder
		//

		public CallbackInfo WithCallback( Action callbackEvent )
		{
			_callback = callbackEvent;
			return this;
		}

		public CallbackInfo FromType( Type type )
		{
			Class = type;
			return this;
		}

		// Group
		public class Group : List<CallbackInfo> { }
	}
}
