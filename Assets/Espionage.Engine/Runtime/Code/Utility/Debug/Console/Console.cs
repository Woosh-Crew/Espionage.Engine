using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Espionage.Engine.Internal;

using Debug = UnityEngine.Debug;
using System.Threading.Tasks;

namespace Espionage.Engine
{
	// [Manager( nameof( Initialize ), Layer = Layer.Runtime | Layer.Editor )]
	public static partial class Console
	{
		public struct Command
		{
			public string Name { get; internal set; }
			public string Help { get; internal set; }
			public Layer Layer { get; internal set; }

			private Action<object[]> _action;
			public MemberInfo Info { get; internal set; }

			public Command WithAction( Action<object[]> action )
			{
				_action = action; return this;
			}
			public void Invoke( object[] args ) => _action?.Invoke( args );
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

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterSceneLoad )]
		internal async static void Initialize()
		{
			using ( _ = new TimedScope( "Finished Initializing console" ) )
			{
				_commandProvider = new CommandProvider();

				// We use a task so we can quick load
				var task = Task.Run( () =>
				{
					// Get every CmdAttribute using Linq
					var types = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany( e => e.GetTypes()
										.SelectMany( e => e.GetMembers( BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic )
										.Where( e => e.IsDefined( typeof( CmdAttribute ) ) ) ) );

					foreach ( var info in types )
					{
						foreach ( var item in info.GetCustomAttribute<CmdAttribute>().Create( info ) )
							_commandProvider.Add( item );
					}
				} );

				await task;

				Debug.Log( $"Commands: {_commandProvider.All.Count}" );

				Application.logMessageReceived += UnityLogHook;
			}
		}

		//
		// Logging
		//

		public static IReadOnlyList<Entry> Logs => _logs;
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
			var layer = Layer.Runtime;

			if ( stackTrace.Contains( "UnityEditor" ) || stackTrace.Contains( "Editor" ) )
				layer = Layer.Editor;

			AddLog( new Entry( logString, stackTrace, layer, type ) );
		}

		//
		// Commands
		//

		internal static ICommandProvider _commandProvider;

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
