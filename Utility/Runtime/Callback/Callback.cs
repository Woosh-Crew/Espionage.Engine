using System;
using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Internal.Callbacks;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Editor | Layer.Runtime, Order = 50 )]
	public static partial class Callback
	{
		private static ICallbackProvider Provider { get; set; }

		internal static void Initialize()
		{
			using ( Debugging.Stopwatch( "Callbacks Initialized" ) )
			{
				// AttributeCallbackProvider is the default provider
				Provider ??= new AttributeCallbackProvider();
			}
		}

		/// <summary> Runs a callback with an array of args. </summary>
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

		/// <summary> Runs a callback that returns a value </summary>
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
			catch ( KeyNotFoundException )
			{
				Debugging.Log.Error( $"Key : {name}, somehow couldn't be found or created?" );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}

			return null;
		}

		/// <summary> Register an object to receive callbacks </summary>
		public static void Register( ICallbacks item )
		{
			if ( item is null )
			{
				return;
			}

			Provider?.Register( item );
		}

		/// <summary> Unregister an object to stop receiving callbacks </summary>
		public static void Unregister( ICallbacks item )
		{
			if ( item is null )
			{
				return;
			}

			Provider?.Unregister( item );
		}
	}
}
