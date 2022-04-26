using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Espionage.Engine.Logging;
using Espionage.Engine.Commands;
using Espionage.Engine.Overlays;
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
		/// <summary>
		/// Is Developer is a launch arg -dev which indicated should
		/// we enable developer features.
		/// </summary>
		public static bool IsDeveloper { get; }

		// Providers

		/// <summary>
		/// Command Console. Use Run(string, object[]) to run a command.
		/// Your game can have its own Console provider.
		/// </summary>
		public static ICommandProvider Terminal { get; set; }

		/// <summary>
		/// Add your own extension methods if need be, since this is an
		/// instanced class. 
		/// </summary>
		public static ILoggingProvider Log { get; set; }

		/// <summary>
		/// Debug overlays for positional debugging and screen text.
		/// Instanced so you can provide you're own provider
		/// or extension methods.
		/// </summary>
		public static IOverlayProvider Overlay { get; set; }

		/// <summary>
		/// Runs a stopwatch on a IDisposable Scope. Use this in a using() expression
		/// to record how long it took to execute that code block.
		/// </summary>
		public static IDisposable Stopwatch( string message = null, bool alwaysReport = false )
		{
			return ReportStopwatch || alwaysReport ? new TimedScope( message ) : null;
		}

		static Debugging()
		{
			IsDeveloper = Application.isEditor || Environment.GetCommandLineArgs().Contains( "-dev" );

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

				// Get Pipe Handle and start IPC
				if ( args[i].StartsWith( "-connect" ) )
				{
					Messages.Connect( args[i + 1] );
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
		private static void Help( string input = null )
		{
			var builder = new StringBuilder();

			foreach ( var item in string.IsNullOrWhiteSpace( input ) ? Terminal.All : Terminal.Find( input ) )
			{
				builder.AppendLine( item.Member.Name );

				if ( string.IsNullOrWhiteSpace( item.Member.Help ) )
				{
					continue;
				}

				builder.Append( $" : {item.Member.Help}" );
			}

			Log.Add( new() { Message = builder.ToString(), Level = "Response" } );
		}

		[Terminal, Function( "clear" )]
		private static void Clear()
		{
			Log.Clear();
		}

		[Terminal, Function( "quit" )]
		private static void Quit()
		{
			Application.Quit();
		}

		// Debug

		[Terminal, Property( "dev.report_stopwatch", true )]
		private static bool ReportStopwatch { get; set; } = true;

		[Terminal, Property( "dev.overlays", true )]
		private static bool ShowOverlays
		{
			get => Overlay?.Show ?? false;
			set
			{
				if ( Overlay == null )
				{
					return;
				}

				Overlay.Show = value;
			}
		}

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
