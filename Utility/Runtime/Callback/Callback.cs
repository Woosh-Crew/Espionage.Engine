using System;
using System.Collections.Generic;
using System.Linq;
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

		// Initialization Queue

		private static Queue<QueuedCallback> _callbackQueue = new();
		private static ICallbackProvider Provider { get; set; }

		[Debugging.Var( "callbacks.report" )]
		private static bool Report { get; } = false;

		internal static async void Initialize()
		{
			using ( Debugging.Stopwatch( "Callbacks Initialized" ) )
			{
				_isInitializing = true;

				// AttributeCallbackProvider is the default provider
				Provider ??= new AttributeCallbackProvider();

				_isInitializing = false;

				// Dequeue any callbacks called when initializing
				for ( var i = 0; i < _callbackQueue.Count; i++ )
				{
					var item = _callbackQueue.Dequeue();
					Run( item.Name, item.Args );
				}

				_callbackQueue.Clear();
				_callbackQueue = null;
			}
		}

		//
		// API
		//

		/// <summary>
		///     <inheritdoc cref="Callback.Run(string, object[])" /> With the option of it being unreliable.
		///     Which makes it so it wont run if the callback is initializing or something goes wrong.
		/// </summary>
		public static void Run( string name, bool unreliable, params object[] args )
		{
			if ( unreliable && _isInitializing )
			{
				return;
			}

			Run( name, args );
		}

		/// <summary> Runs a callback with an array of args. </summary>
		public static void Run( string name, params object[] args )
		{
			if ( Provider is null || string.IsNullOrEmpty( name ) )
			{
				return;
			}

			// Queue if we are still initializing
			if ( _isInitializing )
			{
				_callbackQueue.Enqueue( new QueuedCallback
				{
					Name = name,
					Args = args
				} );
				Debugging.Log.Warning( $"Callbacks are still initializing, Queuing: {name}" );
				return;
			}

			if ( Report )
			{
				Debugging.Log.Info( $"Invoking Callback: {name}" );
			}

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
			{
				return null;
			}

			if ( _isInitializing )
			{
				throw new Exception( "Can't run a return value callback while initializing" );
			}

			if ( Report )
			{
				Debugging.Log.Info( $"Invoking Return Value Callback: {name}" );
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

		private struct QueuedCallback
		{
			public string Name;
			public object[] Args;
		}

		//
		// Callback Var
		//

		public class Var<T>
		{
			public Var( string eventName )
			{
				_callback = ( oldValue, newValue ) =>
				{
					Run( eventName, oldValue, newValue );
				};
			}

			public Var( Action<T, T> callback )
			{
				_callback = callback;
			}

			private T _value;

			private T Value
			{
				set
				{
					if ( value.Equals( _value ) )
					{
						return;
					}


					_callback?.Invoke( _value, value );
					_value = value;
				}
				get => _value;
			}

			//
			// Callback Event
			//

			private readonly Action<T, T> _callback;

			//
			// Operators
			//

			public static implicit operator T( Var<T> callbackVar )
			{
				return callbackVar.Value;
			}
		}
	}
}
