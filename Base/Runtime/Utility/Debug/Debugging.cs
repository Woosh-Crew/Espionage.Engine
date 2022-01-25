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
			Log ??= new SimpleLoggingProvider();

			Log.Initialize();

			using ( Stopwatch( "Debugging Initialized" ) )
			{
				// Setup Console
				Console ??= new AttributeCommandProvider<CmdAttribute>();

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

		[Var( "debug.report_stopwatch" )]
		private static bool ReportStopwatch { get; set; } = false;

		public static IDisposable Stopwatch( string message = null, bool alwaysReport = false )
		{
			return ReportStopwatch || alwaysReport ? new TimedScope( message, 0 ) : null;
		}

		public static IDisposable Stopwatch( string message, int reportIfOverTime )
		{
			return new TimedScope( message, reportIfOverTime );
		}

		internal class TimedScope : IDisposable
		{
			private readonly Stopwatch _stopwatch;
			private readonly string _message;
			private readonly int _reportTime;

			public TimedScope( string message, int reportTime )
			{
				_message = message;
				_reportTime = reportTime;
				_stopwatch = System.Diagnostics.Stopwatch.StartNew();
			}

			public void Dispose()
			{
				_stopwatch.Stop();

				if ( _stopwatch.ElapsedMilliseconds <= _reportTime )
				{
					return;
				}

				var time = $"{_stopwatch.ElapsedMilliseconds}ms";

				if ( string.IsNullOrEmpty( _message ) )
				{
					Log.Info( time );
					return;
				}

				Log.Info( $"{string.Format( _message )} | {time}" );
			}
		}
	}
}
