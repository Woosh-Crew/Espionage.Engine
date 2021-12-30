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
			public Type Owner { get; internal set; }

			private Action<object[]> _action;
			public MemberInfo Info { get; internal set; }

			public Command WithAction( Action<object[]> action )
			{
				_action = action;
				return this;
			}

			public void Invoke( object[] args ) => _action?.Invoke( args );
		}

		internal static IConsoleProvider Provider { get; set; }

		internal async static void Initialize()
		{
			using ( Debugging.Stopwatch( "Console System Initialized" ) )
			{
				Provider = new RuntimeConsoleProvider( new AttributeCommandProvider<Console.CmdAttribute>() );
				await Provider.Initialize();
			}

			// Testing
			Invoke( "help" );
		}

		//
		// Commands
		//

		public static void Invoke( string commandLine ) => Provider?.CommandProvider?.Invoke( commandLine );
		public static void Invoke( string command, params string[] args ) => Provider?.CommandProvider?.Invoke( command, args );

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
