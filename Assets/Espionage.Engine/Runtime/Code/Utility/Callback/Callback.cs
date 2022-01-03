// Attribute based event callback system

using System;
using System.Collections.Generic;
using Espionage.Engine.Internal.Callbacks;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Order = 50 )]
	public static partial class Callback
	{
		//
		// Initialize
		//

		public static async void Initialize()
		{
			using ( Debugging.Stopwatch( "Callbacks Initialized" ) )
			{
				if ( Provider is null )
					Provider = new AttributeCallbackProvider();

				await Provider.Initialize();
			}

			Run( "test", "penis?" );
		}

		[Callback( "test" )]
		public static void Test( string fuck )
		{
			Debugging.Log.Info( "Worked" );
			Debugging.Log.Info( fuck );
		}

		//
		// API
		//

		public static void Run( string name, params object[] args )
		{
			try
			{
				Provider?.Run( name, args );
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
