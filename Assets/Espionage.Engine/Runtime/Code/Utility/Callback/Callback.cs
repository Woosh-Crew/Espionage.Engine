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
				// AttributeCallbackProvider is the default provider
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

		/// <summary> Runs a callback </summary>
		public static void Run( string name, params object[] args )
		{
			if ( Provider is null || string.IsNullOrEmpty( name ) )
				return;

			try
			{
				Provider.Run( name, args );
			}
			catch ( KeyNotFoundException )
			{
				Debugging.Log.Error( $"Key : {name}, somehow couldn't be found or created?" );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}
		}

		/// <summary> Runs a callback that returns a value </summary>
		public static T[] Run<T>( string name, params object[] args )
		{
			throw new NotImplementedException();
		}

		/// <summary> Register an object to recieve callbacks </summary>
		public static void Register( ICallbacks item )
		{
			if ( item is null )
				return;

			Provider?.Register( item );
		}


		/// <summary> Unregister an object to stop receiving callbacks </summary>
		public static void Unregister( ICallbacks item )
		{
			if ( item is null )
				return;

			Provider?.Unregister( item );
		}

		//
		// Provider
		//

		internal static ICallbackProvider Provider { get; set; }
	}
}
