using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Espionage.Engine.Internal;

using Espionage.Engine.Internal.Logging;
using Espionage.Engine.Internal.Commands;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Runtime | Layer.Editor, Order = -200 )]
	public static partial class Debugging
	{
		//
		// Initialize
		//

		public static async void Initialize()
		{
			// We initialize logging without
			// Async so we can log straight away

			if ( Log is null )
				Log = new SimpleLoggingProvider();

			Log.Initialize();

			using ( Debugging.Stopwatch( "Debugging Initialized" ) )
			{
				// Setup Console
				if ( Console is null )
					Console = new AttributeCommandProvider<CmdAttribute>();

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

		[Debugging.Var( "debug.report_stopwatch" )]
		public static bool ReportStopwatch { get; set; } = true;

		public static IDisposable Stopwatch( string message = null, params object[] args )
		{
			if ( ReportStopwatch )
				return new TimedScope( message, args );
			else
				return null;
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

				var time = $"{_stopwatch.ElapsedMilliseconds}ms";

				if ( string.IsNullOrEmpty( _message ) )
				{
					Log.Info( time );
					return;
				}

				Log.Info( $"{String.Format( _message, _args )} | {time}" );
			}
		}
	}
}
