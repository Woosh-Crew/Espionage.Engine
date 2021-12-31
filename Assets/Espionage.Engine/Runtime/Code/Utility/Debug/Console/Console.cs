using System;
using System.Collections.Generic;
using System.Reflection;
using Espionage.Engine.Internal;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ) )]
	public static partial class Console
	{
		internal static bool HasInitialized { get; private set; }

		public static IConsoleProvider Provider { get; set; }

		public async static void Initialize()
		{
			if ( HasInitialized )
				return;

			// Do this here cause of race conditions
			HasInitialized = true;

			using ( Debugging.Stopwatch( "Console System Initialized - " + (Provider is null ? "Default Provider" : $"[{Provider.GetType().Name}]") ) )
			{
				// If we have no provider already, just use the default one
				if ( Provider is null )
					Provider = new RuntimeConsoleProvider( new AttributeCommandProvider<Console.CmdAttribute>() );

				await Provider.Initialize();
			}
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
			object[] finalArgs = new object[paramters.Length];

			for ( int i = 0; i < MathF.Min( paramters.Length, args.Length ); i++ )
			{
				finalArgs[i] = System.Convert.ChangeType( args[i], paramters[i] );
			}

			return finalArgs;
		}

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
	}
}
