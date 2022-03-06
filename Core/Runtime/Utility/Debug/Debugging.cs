using System;
using System.Diagnostics;
using Espionage.Engine.Internal.Logging;
using Espionage.Engine.Internal.Commands;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine's core Debugging Library. Has support for
	/// logging, commands, overlays, and other utility features.
	/// You should be using this over Unity's debug library.
	/// </summary>
	[Library, Group( "Debug" ), Manager( nameof( Initialize ), Layer = Layer.Runtime | Layer.Editor, Order = -200 )]
	public static class Debugging
	{
		// Providers

		/// <summary>
		/// Command Console. Use Run(string, object[]) to run a command. Reason
		/// its not its own static class is so we can add extension methods to it.
		/// It also provides a SOLID way of handling it. Your game can have its own
		/// Console provider.
		/// </summary>
		public static ICommandProvider Console { get; private set; }

		/// <summary>
		/// Logging in a SOLID way. Add your own extension methods if need be,
		/// since this is an instanced class.
		/// </summary>
		public static ILoggingProvider Log { get; private set; }

		/// <summary>
		/// Draw Debug Overlays on the Viewport, such as spheres, cubes, etc.
		/// Very useful for debugging volumes and collisions.
		/// </summary>
		public static IDebugOverlayProvider Overlay => throw new NotImplementedException();

		// Stopwatch

		/// <summary>
		/// Runs a stopwatch on a IDisposable Scope. Use this in a using() expression
		/// to record how long it took to execute that code block.
		/// </summary>
		/// <param name="message">The message that should print along side the completion time.</param>
		/// <param name="alwaysReport">Should we always report? or only report if the Var is set.</param>
		public static IDisposable Stopwatch( string message = null, bool alwaysReport = false )
		{
			return ReportStopwatch || alwaysReport ? new TimedScope( message, 0 ) : null;
		}

		/// <summary>
		/// <inheritdoc cref="Stopwatch(string,bool)"/>
		/// </summary>
		/// <param name="message"><inheritdoc cref="Stopwatch(string,bool)"/></param>
		/// <param name="reportIfOverTime">If the stopwatch goes past this time, we will report it.</param>
		public static IDisposable Stopwatch( string message, int reportIfOverTime )
		{
			return new TimedScope( message, reportIfOverTime );
		}

		//
		// Initialize
		//

		private static void Initialize()
		{
			using ( Stopwatch( "Debugging Initialized" ) )
			{
				Log ??= new SimpleLoggingProvider();
				Console ??= new SimpleCommandProvider();

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
		}

		//
		// Commands
		//

		// App

		[ConVar, Property( "application.product_name" )]
		private static string ProductName => Application.productName;

		[ConVar, Property( "application.company_name" )]
		private static string CompanyName => Application.companyName;

		[ConVar, Property( "application.unity_version" )]
		private static string UnityVersion => Application.unityVersion;

		[ConVar, Property( "application.sys_language" )]
		private static string SystemLang => Application.systemLanguage.ToString();

		[ConVar, Property( "application.target_fps" ), Cookie]
		private static int TargetFramerate { get => Application.targetFrameRate; set => Application.targetFrameRate = value; }

		// Debug

		[ConVar, Property( "debug.overlay" )]
		private static bool ShowOverlays { get => Overlay.Show; set => Overlay.Show = value; }

		[ConVar, Property( "debug.report_stopwatch" )]
		private static bool ReportStopwatch { get; set; } = true;

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

				if ( _reportTime != 0 && _stopwatch.ElapsedMilliseconds <= _reportTime )
				{
					return;
				}

				var time = _stopwatch.ElapsedMilliseconds > 0 ? $"{_stopwatch.ElapsedMilliseconds}ms" : $"{_stopwatch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency * 1000000000}ns";
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
