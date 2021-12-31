using System;
using System.Collections.Generic;
using System.Reflection;
using Espionage.Engine.Internal;
using UnityEngine; // We should use our own logging

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ) )]
	public static partial class Console
	{
		internal static bool HasInitialized { get; private set; }

		public async static void Initialize()
		{
			if ( HasInitialized )
				return;

			// If we have no provider already, just use the default one,
			// we return because providers set initializes for us.
			if ( Provider is null )
			{
				Provider = new RuntimeConsoleProvider( new AttributeCommandProvider<Console.CmdAttribute>() );
				return;
			}

			// Do this here cause of race conditions
			HasInitialized = true;

			using ( Debugging.Stopwatch( "Console System Initialized - " + (Provider is null ? "Default Provider" : $"[{Provider.GetType().Name}]") ) )
			{
				await Provider.Initialize();
			}
		}

		//
		// Provider
		//

		private static IConsoleProvider _provider;

		public static IConsoleProvider Provider
		{
			internal get
			{
				return _provider;
			}

			set
			{
				HasInitialized = false;
				_provider = value;
				Initialize();
			}
		}

		//
		// Commands
		//

		public static void Invoke( string commandLine ) => Provider.CommandProvider?.Invoke( commandLine );
		public static void Invoke( string command, params string[] args ) => Provider.CommandProvider?.Invoke( command, args );

		internal static void SetupDefaults()
		{
			// Initialize default commands from scratch, that way they are present
			// on every ICommandProvider.
			var quitCmd = new Console.Command()
			{
				Name = "quit",
				Help = "Quits the game",
				Owner = typeof( Console )
			};

			quitCmd.WithAction( ( e ) => QuitCmd() );
			Provider.CommandProvider?.Add( quitCmd );

			var helpCmd = new Console.Command()
			{
				Name = "help",
				Help = "Dumps all commands, or anything starting with input",
				Owner = typeof( Console )
			};

			helpCmd.WithAction( ( e ) => HelpCmd() );
			Provider.CommandProvider?.Add( helpCmd );
		}

		private static void HelpCmd()
		{
			Debug.Log( $"Commands" );

			foreach ( var item in Console.Provider.CommandProvider?.All )
				Debug.Log( $"{item.Name} - {item.Owner.FullName}" );
		}

		private static void QuitCmd()
		{
			Application.Quit();
		}

		//
		// Logging
		//

		public static void Log( string message )
		{

		}

		// 
		// Interpreter
		//

		internal static object[] ConvertArgs( MemberInfo info, string[] args )
		{
			// Set all args to lowercase
			for ( int i = 0; i < args.Length; i++ )
			{
				args[i] = args[i].ToLower();
			}

			// If were a property - Only convert first arg
			if ( info is PropertyInfo property )
			{
				object value = null;

				if ( args.Length > 0 )
					value = Convert.ChangeType( args[0], property.PropertyType );

				return new object[] { value };
			}

			// Now if were a method, convert all args
			else if ( info is MethodInfo method )
			{
				var parameters = method.GetParameters();
				object[] finalArgs = new object[parameters.Length];

				for ( int i = 0; i < parameters.Length; i++ )
				{
					finalArgs[i] = i >= args.Length ? parameters[i].DefaultValue : System.Convert.ChangeType( args[i], parameters[i].ParameterType );
				}

				return finalArgs;
			}

			throw new InvalidProgramException( $"Convert Args doesnt support info type, {info.GetType().Name}" );
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
