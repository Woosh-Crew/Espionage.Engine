using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace Espionage.Engine
{
	public static class Console
	{
		public enum Layer { Runtime, Editor, Both }

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

		public static bool GameRunning => Application.isPlaying;

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
#else

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
#endif
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


		[Console.Cmd( "clear", Layer = Layer.Both )]
		public static void ClearLogs()
		{
			logs.Clear();
			OnClear?.Invoke();
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
			if ( (consoleCommand.Layer is Layer.Both || (Application.isEditor && consoleCommand.Layer is Layer.Editor) || (GameRunning && consoleCommand.Layer is Layer.Runtime)) )
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

		[Console.Cmd( "help", Layer = Layer.Both )]
		public static void HelpCmd()
		{
			foreach ( var item in commands.Values )
			{
				if ( !Application.isEditor && item.Layer is Layer.Editor )
					continue;

				AddLog( new Entry( $"{item.Name}", "", Layer.Both, LogType.Log ) );
			}
			AddLog( new Entry( "Commands", "", Layer.Both, LogType.Log ) );
		}

		// 
		// Factory
		//

		[System.AttributeUsage( System.AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
		public class CmdAttribute : System.Attribute
		{
			readonly string name;

			public string Name => name;
			public string Help { get; set; }
			public Layer Layer { get; set; } = Layer.Runtime;

			public CmdAttribute( string name )
			{
				this.name = name;
			}

			public virtual Command CreateCommand( MemberInfo info )
			{
				var method = info as MethodInfo;

				return new Command()
				{
					Name = this.Name,
					Help = this.Help,
					Layer = this.Layer,
					OnInvoke = ( e ) => method.Invoke( null, e ),
					Info = info,
				};
			}
		}


		[System.AttributeUsage( System.AttributeTargets.Property, Inherited = false, AllowMultiple = false )]
		public sealed class VarAttribute : CmdAttribute
		{
			public bool IsReadOnly { get; set; }

			public VarAttribute( string name ) : base( name ) { }

			public override Command CreateCommand( MemberInfo info )
			{
				var property = info as PropertyInfo;

				return new Command()
				{
					Name = this.Name,
					Help = this.Help,

					OnInvoke = ( parameters ) =>
				 	{
						 if ( !IsReadOnly && parameters is not null && parameters.Length > 0 )
						 {
							 property.SetValue( null, parameters[0] );
							 Debug.Log( $"{Name} is now {property.GetValue( null )}" );
						 }
						 else
						 {
							 Debug.Log( $"{Name} = {property.GetValue( null )}" );
						 }
				 	},

					Info = info,
				};
			}
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
