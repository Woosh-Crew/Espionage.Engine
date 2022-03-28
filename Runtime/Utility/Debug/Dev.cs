using System;
using System.Diagnostics;
using System.Text;
using Espionage.Engine.Logging;
using Espionage.Engine.Internal.Commands;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine's core Debugging Library. Has support for
	/// logging, commands, overlays, and other utility features.
	/// You should be using this over Unity's debug library.
	/// </summary>
	[Library, Group( "Debug" )]
	public static class Dev
	{
		// Providers

		/// <summary>
		/// Command Console. Use Run(string, object[]) to run a command. Reason
		/// its not its own static class is so we can add extension methods to it.
		/// It also provides a SOLID way of handling it. Your game can have its own
		/// Console provider.
		/// </summary>
		public static ICommandProvider Terminal { get; set; }

		/// <summary>
		/// Logging in a SOLID way. Add your own extension methods if need be,
		/// since this is an instanced class.
		/// </summary>
		public static ILoggingProvider Log { get; set; }

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

		static Dev()
		{
			Log = new SimpleLoggingProvider();
			Terminal = new SimpleCommandProvider();
		}

		//
		// Launch Args
		//

		[Function, Callback( "engine.ready" )]
		private static void ParseLaunchArgs()
		{
			// We don't care, we're in the stupid editor
			if ( Application.isEditor )
			{
				return;
			}

			var args = Environment.GetCommandLineArgs();

			for ( var i = 0; i < args.Length; i++ )
			{
				if ( args[i].StartsWith( "+" ) )
				{
					Terminal.Invoke( args[i][1..], new[] { args[i + 1] } );
				}
			}
		}

		//
		// Commands
		//

		// App

		[Terminal, Property( "application.unity_version" )]
		private static string Version => Application.unityVersion;

		[Terminal, Property( "application.sys_language" )]
		private static string Language => Application.systemLanguage.ToString();

		[Terminal, Function( "help" )]
		private static void Help()
		{
			var builder = new StringBuilder();

			foreach ( var item in Terminal.All )
			{
				builder.AppendLine( $"{item.Name} : {item.Help}" );
			}

			Log.Add( new() { Message = builder.ToString(), Level = "Response" } );
		}

		[Terminal, Function( "clear" )]
		private static void Clear()
		{
			Log.Clear();
		}

		// Debug

		[Terminal, Property( "dev.report_stopwatch", true )]
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
