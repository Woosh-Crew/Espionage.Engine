using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Espionage.Engine.Internal;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ) )]
	public static partial class Debugging
	{
		//
		// Initialize
		//

		public static async void Initialize()
		{
			// Init Logging
			Log.Initialize();

			using ( Debugging.Stopwatch( "Debugging Initialized" ) )
			{
				await Task.WhenAll( Console.Initialize() );
			}
		}

		//
		// Console
		//

		public static ICommandProvider Console { get; private set; }

		//
		// Logging
		//

		public static ILoggingProvider Log { get; private set; }

		//
		// Stopwatch
		//

		public static IDisposable Stopwatch( string message = null, params object[] args )
		{
			return new TimedScope( message, args );
		}

		internal class TimedScope : IDisposable
		{
			private Stopwatch _stopwatch;
			private string _message;
			private object[] _args;

			public TimedScope( string message, params object[] args )
			{
				_stopwatch = System.Diagnostics.Stopwatch.StartNew();
				_message = message;
				_args = args;
			}

			public void Dispose()
			{
				_stopwatch.Stop();

				if ( string.IsNullOrEmpty( _message ) )
				{
					Log.Info( $"{_stopwatch.ElapsedMilliseconds}ms" );
					return;
				}

				Log.Info( $"{String.Format( _message, _args )} | {_stopwatch.ElapsedMilliseconds}ms" );
			}
		}

	}
}
