// Attribute based event callback system

using System;
using System.Linq;
using System.Collections.Generic;
using Espionage.Engine.Internal.Callbacks;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Editor | Layer.Runtime, Order = 50 )]
	public static partial class Callback
	{
		//
		// Initialize
		//

		private static bool _isInitializing;

		public static async void Initialize()
		{
			using ( Debugging.Stopwatch( "Callbacks Initialized" ) )
			{
				_isInitializing = true;

				// AttributeCallbackProvider is the default provider
				if ( Provider is null )
					Provider = new AttributeCallbackProvider();

				await Provider.Initialize();

				_isInitializing = false;
			}

			// Dequeue any callbacks called when initializing
			for ( int i = 0; i < _callbackQueue.Count; i++ )
			{
				var item = _callbackQueue.Dequeue();
				Run( item.name, item.args );
			}

			_callbackQueue.Clear();
			_callbackQueue = null;
		}

		// Initialization Queue

		private static Queue<QueuedCallback> _callbackQueue = new Queue<QueuedCallback>();

		private struct QueuedCallback
		{
			public string name;
			public object[] args;
		}

		[Debugging.Var( "callbacks.report" )]
		private static bool Report { get; set; } = true;

		//
		// API
		//

		/// <summary> Runs a callback </summary>
		public static void Run( string name, params object[] args )
		{
			if ( Provider is null || string.IsNullOrEmpty( name ) )
				return;

			// Queue if we are still init'in
			if ( _isInitializing )
			{
				_callbackQueue.Enqueue( new QueuedCallback() { name = name, args = args } );
				Debugging.Log.Warning( $"Callbacks are still initializing, Queuing: {name}" );
				return;
			}

			if ( Report )
				Debugging.Log.Info( $"Invoking Callback: {name}" );

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
		public static IEnumerable<T> Run<T>( string name, params object[] args )
		{
			if ( Provider is null || string.IsNullOrEmpty( name ) )
				return null;

			if ( _isInitializing )
				throw new Exception( "Can't run a return value callback while initializing" );

			if ( Report )
				Debugging.Log.Info( $"Invoking Return Value Callback: {name}" );

			try
			{
				return Provider.Run( name, args ).Cast<T>();
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
