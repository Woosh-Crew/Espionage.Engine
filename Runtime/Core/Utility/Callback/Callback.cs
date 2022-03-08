using System;
using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Internal.Callbacks;

namespace Espionage.Engine
{
	public static class Callback
	{
		public static ICallbackProvider Provider { get; set; }

		static Callback()
		{
			// AttributeCallbackProvider is the default provider
			Provider = new AttributeCallbackProvider();
		}

		/// <summary> Runs a callback with an array of args. </summary>
		public static void Run( string name )
		{
			if ( Provider is null || string.IsNullOrEmpty( name ) )
			{
				return;
			}

			Provider.Run( name );
		}

		/// <summary> Runs a callback with an array of args. [EXPENSIVE] </summary>
		public static void Run( string name, params object[] args )
		{
			if ( Provider is null || string.IsNullOrEmpty( name ) )
			{
				return;
			}

			try
			{
				Provider.Run( name, args );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}
		}

		/// <summary> Runs a callback that returns a value. [EXPENSIVE] </summary>
		public static IEnumerable<T> Run<T>( string name, params object[] args )
		{
			if ( Provider is null || string.IsNullOrEmpty( name ) )
			{
				return null;
			}

			try
			{
				var values = Provider.Run( name, args );
				return values?.Cast<T>();
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}

			return null;
		}

		/// <summary> Register an object to receive callbacks </summary>
		public static void Register( ILibrary item )
		{
			if ( item is null )
			{
				return;
			}

			Provider?.Register( item );
		}

		/// <summary> Unregister an object to stop receiving callbacks </summary>
		public static void Unregister( ILibrary item )
		{
			if ( item is null )
			{
				return;
			}

			Provider?.Unregister( item );
		}

		internal struct Info
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

			public Info WithCallback( Action callbackEvent )
			{
				_callback = callbackEvent;
				return this;
			}

			public Info FromType( Type type )
			{
				Class = type;
				return this;
			}

			// Group
			public class Group : List<Info> { }
		}
	}
}
