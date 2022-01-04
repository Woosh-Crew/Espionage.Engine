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

			// If we are exiting playmode, dispose the provider
#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += (e =>
			{
				if ( e is UnityEditor.PlayModeStateChange.ExitingPlayMode )
					Provider?.Dispose();
			});
#endif
		}

		//
		// API
		//

		public static void Run( string name, params object[] args )
		{
			if ( Provider is null )
				return;

			try
			{
				Provider.Run( name, args );
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
