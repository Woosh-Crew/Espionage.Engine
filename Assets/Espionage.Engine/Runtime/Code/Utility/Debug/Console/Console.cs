using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Espionage.Engine.Internal;

using Debug = UnityEngine.Debug;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ) )]
	public static partial class Console
	{
		public struct Command
		{
			public string Name { get; internal set; }
			public string Help { get; internal set; }

			private Action<object[]> _action;
			public MemberInfo Info { get; internal set; }

			public Command WithAction( Action<object[]> action )
			{
				_action = action;
				return this;
			}
			public void Invoke( object[] args ) => _action?.Invoke( args );
		}

		public struct Entry
		{
			public Entry( string message, string trace, LogType type )
			{
				Message = message;
				Trace = trace;
				Type = type;
			}

			public LogType Type { get; internal set; }
			public string Message { get; internal set; }
			public string Trace { get; internal set; }
		}

		//
		// System
		//

		internal async static void Initialize()
		{
			using ( Debugging.Stopwatch( "Console Initialized" ) )
			{
				// Commands
				_commandProvider = new AttributeCommandProvider<Console.CmdAttribute>();

				await _commandProvider?.Initialize();

				// Initialize default commands from scratch, that way they are present
				// on every ICommandProvider.
				var quitCmd = new Command() { Name = "quit", Help = "Quits the game" };
				quitCmd.WithAction( ( e ) => QuitCmd() );
				_commandProvider?.Add( quitCmd );

				var clearCmd = new Command() { Name = "clear", Help = "Clears all logs" };
				clearCmd.WithAction( ( e ) => ClearCmd() );
				_commandProvider?.Add( clearCmd );

				var helpCmd = new Command() { Name = "help", Help = "Dumps all commands, or anything starting with input" };
				helpCmd.WithAction( ( e ) => HelpCmd() );
				_commandProvider?.Add( helpCmd );

				Debug.Log( $"Found {_commandProvider.All.Count} Commands" );

				// Run all launch args
				foreach ( var item in System.Environment.GetCommandLineArgs() )
					_commandProvider?.LaunchArgs( item );

				Application.logMessageReceived += UnityLogHook;
			}
		}

		//
		// Logging
		//

		public static IReadOnlyCollection<Entry> Logs => _logs;
		private static List<Entry> _logs = new List<Entry>();

		public static Action<Entry> OnLog;
		public static Action OnClear;


		public static void AddLog( Entry entry )
		{
			_logs.Add( entry );
			OnLog?.Invoke( entry );
		}

		private static void UnityLogHook( string logString, string stackTrace, LogType type )
		{
			AddLog( new Entry( logString, stackTrace, type ) );
		}

		//
		// Commands
		//

		internal static ICommandProvider _commandProvider;

		public static void Invoke( string commandLine ) => _commandProvider?.Invoke( commandLine );
		public static void Invoke( string command, params string[] args ) => _commandProvider?.Invoke( command, args );

		// 
		// Interpreter
		//

		internal static object[] ConvertArgs( Type[] paramters, string[] args )
		{
			List<object> finalArgs = new List<object>();

			for ( int i = 0; i < args.Length; i++ )
			{
				finalArgs.Add( System.Convert.ChangeType( args[i], paramters[i] ) );
			}

			return finalArgs.ToArray();
		}

		public static Type[] GetParameterTypes( this MemberInfo info )
		{
			List<Type> paramteres = new List<Type>();

			if ( info is PropertyInfo propertyInfo )
				return new Type[] { propertyInfo.PropertyType };

			if ( info is MethodInfo methodInfo )
			{
				foreach ( var item in methodInfo.GetParameters() )
					paramteres.Add( item.ParameterType );
			}

			return paramteres.ToArray();
		}

		// Copied from this (https://stackoverflow.com/a/2132004)
		public static string[] SplitArguments( this string commandLine )
		{
			var parmChars = commandLine.ToCharArray();
			var inSingleQuote = false;
			var inDoubleQuote = false;
			for ( var index = 0; index < parmChars.Length; index++ )
			{
				if ( parmChars[index] == '"' && !inSingleQuote )
				{
					inDoubleQuote = !inDoubleQuote;
					parmChars[index] = '\n';
				}
				if ( parmChars[index] == '\'' && !inDoubleQuote )
				{
					inSingleQuote = !inSingleQuote;
					parmChars[index] = '\n';
				}
				if ( !inSingleQuote && !inDoubleQuote && parmChars[index] == ' ' )
					parmChars[index] = '\n';
			}
			return (new string( parmChars )).Split( new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries );
		}
	}
}
