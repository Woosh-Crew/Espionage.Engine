using System;
using System.Diagnostics;
using Espionage.Engine.Internal.Logging;
using Espionage.Engine.Internal.Commands;
using Espionage.Engine.Internal.Overlay;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine's core Debugging Library. Has support for
	/// logging, commands, overlays, and other utility features.
	/// You should be using this over Unity's debug library.
	/// </summary>
	[Library, Group( "Debug" )]
	public static class Debugging
	{
		// Providers

		/// <summary>
		/// Command Console. Use Run(string, object[]) to run a command. Reason
		/// its not its own static class is so we can add extension methods to it.
		/// It also provides a SOLID way of handling it. Your game can have its own
		/// Console provider.
		/// </summary>
		public static ICommandProvider Console { get; set; }

		/// <summary>
		/// Logging in a SOLID way. Add your own extension methods if need be,
		/// since this is an instanced class.
		/// </summary>
		public static ILoggingProvider Log { get; set; }

		/// <summary>
		/// Draw Debug Overlays on the Viewport, such as spheres, cubes, etc.
		/// Very useful for debugging volumes and collisions.
		/// </summary>
		public static IDebugOverlayProvider Overlay { get; set; }

		// Stopwatch

		/// <summary>
		/// Runs a stopwatch on a IDisposable Scope. Use this in a using() expression
		/// to record how long it took to execute that code block.
		/// </summary>
		/// <param name="message">The message that should print along side the completion time.</param>
		/// <param name="alwaysReport">Should we always report? or only report if the Var is set.</param>
		public static IDisposable Stopwatch( string message = null, bool alwaysReport = false )
		{
			return ReportStopwatch || alwaysReport ? new TimedScope( message ) : null;
		}

		//
		// Initialize
		//

		public static bool Initialized { get; private set; }

		internal static void Initialize()
		{
			if ( Initialized )
			{
				return;
			}

			Initialized = true;

			using var _ = Stopwatch( "Debugging Initialized" );

			Log = new SimpleLoggingProvider();
			Console = new SimpleCommandProvider();

			// Setup Default Commands

			// Quit
			Console.Add( new Command()
				{
					Name = "quit",
					Help = "Quits the application"
				}.WithAction( _ => Application.Quit() )
			);

			// Clear
			Console.Add( new Command()
				{
					Name = "clear",
					Help = "Clears everything in the log"
				}.WithAction( _ => Log.Clear() )
			);

			// Help
			Console.Add( new Command()
				{
					Name = "help",
					Help = "Returns Help Message on all commands"
				}.WithAction( _ =>
				{
					Log.Info( "Dumping All" );

					foreach ( var command in Console.All )
					{
						Log.Info( $"{command.Name} = {command.Help}" );
					}
				} )
			);
		}

		//
		// Commands
		//

		// App

		[Terminal, Property( "application.unity_version" )]
		private static string Version => Application.unityVersion;

		[Terminal, Property( "application.sys_language" )]
		private static string Language => Application.systemLanguage.ToString();

		// Debug

		[Terminal, Property( "debug.overlay" )]
		private static bool ShowOverlays
		{
			get => Overlay.Show;
			set => Overlay.Show = value;
		}

		[Terminal, Property( "debug.report_stopwatch", true )]
		private static bool ReportStopwatch { get; set; } = true;

		private class TimedScope : IDisposable
		{
			private readonly Stopwatch _stopwatch;
			private readonly string _message;

			public TimedScope( string message )
			{
				_message = message;

				_stopwatch = System.Diagnostics.Stopwatch.StartNew();
			}

			public void Dispose()
			{
				_stopwatch.Stop();

				var time = _stopwatch.Elapsed.Seconds > 0 ? $"{_stopwatch.Elapsed.TotalSeconds} seconds" : $"{_stopwatch.Elapsed.TotalMilliseconds} ms";

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
