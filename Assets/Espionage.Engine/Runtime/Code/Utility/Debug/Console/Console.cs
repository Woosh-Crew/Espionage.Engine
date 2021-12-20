using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Runtime | Layer.Editor )]
	public static partial class Console
	{
		public struct Command
		{
			public string Name { get; internal set; }
			public string Help { get; internal set; }
			public Layer Layer { get; internal set; }

			public Action<object[]> OnInvoke { get; internal set; }
			public MemberInfo Info { get; internal set; }
		}

		public struct Entry
		{
			public Entry( string message, string trace, Layer layer, LogType type )
			{
				Layer = layer;
				Message = message;
				Trace = trace;
				Type = type;
			}

			public Layer Layer { get; internal set; }
			public LogType Type { get; internal set; }
			public string Message { get; internal set; }
			public string Trace { get; internal set; }
		}

		//
		// System
		//

		internal static void Initialize()
		{
			// Get every CmdAttribute using Linq
			var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany( e => e.GetTypes()
								.SelectMany( e => e.GetMembers( BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic )
								.Where( e => e.IsDefined( typeof( CmdAttribute ) ) ) ) );

			foreach ( var info in types )
				AddCommand( info.GetCustomAttribute<CmdAttribute>().CreateCommand( info ) );

			Debug.Log( $"Console initialized - [Commands: {commands.Count}]" );

			Application.logMessageReceived += UnityLogHook;
		}

		//
		// Logging
		//

		public static IReadOnlyList<Entry> Logs => logs;
		private static List<Entry> logs = new List<Entry>();

		public static Action<Entry> OnLog;
		public static Action OnClear;

		public static void AddLog( Entry entry )
		{
			logs.Add( entry );
			OnLog?.Invoke( entry );
		}

		private static void UnityLogHook( string logString, string stackTrace, LogType type )
		{
			var layer = Layer.Runtime;

			if ( stackTrace.Contains( "UnityEditor" ) || stackTrace.Contains( "Editor" ) )
				layer = Layer.Editor;

			AddLog( new Entry( logString, stackTrace, layer, type ) );
		}

		//
		// Commands
		//

		private static Dictionary<string, Command> commands = new Dictionary<string, Command>( StringComparer.CurrentCultureIgnoreCase );

		public static IReadOnlyCollection<string> History => history;
		private static HashSet<string> history = new HashSet<string>();

		internal static void AddCommand( Command command )
		{
			commands.Add( command.Name, command );
		}

		public static bool InvokeCommand( string commandLine )
		{
			// Only record the history in this method, so ones done programity dont get recorded
			history.Add( commandLine );

			var command = commandLine.Split( ' ' ).First();
			var args = commandLine.Substring( command.Length ).SplitArguments();

			return InvokeCommand( command, args );
		}

		public static bool InvokeCommand( string command, params string[] args )
		{
			if ( !commands.TryGetValue( command, out var consoleCommand ) )
			{
				Debug.Log( $"Couldn't find command \"{command}\"" );
				return false;
			}

			// Check if we are on the correct layer - This looks ultra aids
			if ( (Application.isEditor && consoleCommand.Layer.HasFlag( Layer.Editor )) || (Application.isPlaying && consoleCommand.Layer.HasFlag( Layer.Runtime )) )
			{
				if ( args is not null && args.Length > 0 )
					consoleCommand.OnInvoke.Invoke( ConvertArgs( GetParameterTypes( consoleCommand.Info ), args ) );
				else
					consoleCommand.OnInvoke.Invoke( null );

				return true;
			}

			Debug.Log( $"Trying to invoke command on wrong layer [{consoleCommand.Layer}]" );
			return false;
		}

		public static string[] FindCommand( string input )
		{
			return commands.Keys.Where( e => e.StartsWith( input ) ).ToArray();
		}

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

		internal static Type[] GetParameterTypes( MemberInfo info )
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
		internal static string[] SplitArguments( this string commandLine )
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
