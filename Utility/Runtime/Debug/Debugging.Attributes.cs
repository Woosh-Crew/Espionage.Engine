using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Espionage.Engine.Components;
using Espionage.Engine.Internal.Commands;
using UnityEngine;

namespace Espionage.Engine
{
	public static partial class Debugging
	{
		[AttributeUsage( AttributeTargets.Method, Inherited = false )]
		public class CmdAttribute : Attribute, IComponent<Function>
		{
			public void OnAttached( Function item )
			{
				Log.Info( "Adding Command" );

				if ( !item.Info.IsStatic )
				{
					Log.Error( $"Function \"{item.Name}\" Must be Static!" );
					return;
				}

				foreach ( var command in Create( item ) )
				{
					Console.Add( command );
				}
			}

			public Command Create( IMember info )
			{
				var commands = new List<Command>();

				var helpBuilder = new StringBuilder( info.Help + " - (" );
				var helpMessage = BuildHelpMessage( info, helpBuilder );
				helpBuilder.Append( " )" );

				var command = new Command()
				{
					Name = info.Name,
					Help = helpBuilder.ToString(),
					Info = info as MemberInfo
				};

				OnCreate( ref command, info );

				return command;
			}

			protected virtual void OnCreate( ref Command command, MemberInfo info )
			{
				var method = info as MethodInfo;
				command.WithAction( ( e ) => method?.Invoke( null, e ) );
			}
		}


		/// <summary>
		/// A Var is basically a <see cref="CmdAttribute"/>, with the command prebuilt when initializing.
		/// This var allows you to change and read a property at any time. You can also serialize the value
		/// for persistence. Attribute must be attached to a static property.
		/// </summary>
		[AttributeUsage( AttributeTargets.Property )]
		public sealed class ConVarAttribute : Attribute, IComponent<Property>
		{
			public void OnAttached( Property item )
			{
				if ( !item.IsStatic )
				{
					Log.Error( $"Property \"{item.Name}\" Must be Static!" );
					return;
				}

				var command = new Command()
				{
					Name = item.Name,
					Help = item.Help,
					Info = item.Info
				};

				command.WithAction(
					( parameters ) =>
					{
						if ( parameters is not null && parameters.Length > 0 )
						{
							var value = parameters[0];
							item[null] = value;

							Log.Info( $"{item.Name} is now {value}" );
						}
						else
						{
							Log.Info( $"{item.Name} is now {item[null]}" );
						}
					} );

				Console.Add( command );
			}
		}
	}
}
