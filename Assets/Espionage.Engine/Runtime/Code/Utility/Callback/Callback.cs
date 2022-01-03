// Attribute based event callback system

using System;
using System.Collections.Generic;
using System.Reflection;
using Espionage.Engine.Internal.Callbacks;

namespace Espionage.Engine
{
	public static partial class Callback
	{
		internal struct CallbackInfo
		{
			public Type Class { get; set; }
			public MethodInfo Info { get; set; }
		}

		public static void Run( string name )
		{
			try
			{
				Provider?.Run( name );
			}
			catch ( Exception e )
			{
				Debugging.Log.Error( e.Message );
			}
		}

		public static void Register( object item )
		{
			Provider?.Register( item );
		}


		public static void Unregister( object item )
		{
			Provider?.Unregister( item );
		}

		//
		// Provider
		//

		internal static ICallbackProvider Provider { get; set; }
	}
}
