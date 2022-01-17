using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Espionage.Engine.Internal;
using Espionage.Engine.Internal.Logging;
using Espionage.Engine.Internal.Commands;

namespace Espionage.Engine
{
	[Manager( nameof(Initialize), Layer = Layer.Runtime | Layer.Editor, Order = -200 )]
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
		private static bool ReportStopwatch { get; set; } = true;

		public static IDisposable Stopwatch( string message = null, bool alwaysReport = false )
		{
			if ( ReportStopwatch || alwaysReport )
			{
				return new TimedScope( message );
			}
			else
			{
				return null;
			}
		}

		internal class TimedScope : IDisposable
		{
			private readonly Stopwatch _stopwatch;
			private readonly string _message;

			public TimedScope( string message )
			{
				_stopwatch = System.Diagnostics.Stopwatch.StartNew();
				_message = message;
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

				Log.Info( $"{string.Format( _message )} | {time}" );
			}
		}
	}
}
