using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Espionage.Engine.Internal.Commands;

namespace Espionage.Engine
{
	public static partial class Debugging
	{
		[AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
		public class CmdAttribute : Attribute, ICommandCreator
		{
			private readonly string[] names;

			public string[] Names => names;
			public string Help { get; set; }

			public CmdAttribute( params string[] names )
			{
				this.names = names;
			}

			public Command[] Create( MemberInfo info )
			{
				List<Command> commands = new List<Command>();

				var helpBuilder = new StringBuilder( this.Help + " - (" );
				var helpMessage = BuildHelpMessage( info, helpBuilder );
				helpBuilder.Append( " )" );

				foreach ( var item in Names )
				{
					var command = new Command()
					{
						Name = item,
						Help = helpBuilder.ToString(),
						Owner = info.DeclaringType,
						Info = info,
					};

					OnCreate( ref command, info );
					commands.Add( command );
				}

				return commands.ToArray();
			}

			protected virtual StringBuilder BuildHelpMessage( MemberInfo info, StringBuilder builder )
			{
				if ( info is not MethodInfo method )
					return builder;

				var parameters = method.GetParameters();

				foreach ( var item in parameters )
				{
					if ( item.HasDefaultValue )
					{
						var text = item.DefaultValue is null ? "null" : item.DefaultValue.ToString();
						builder.Append( $" {item.ParameterType.Name} = {text}" );
					}
					else
					{
						builder.Append( $" {item.ParameterType.Name}" );
					}
				}

				return builder;
			}

			protected virtual void OnCreate( ref Command command, MemberInfo info )
			{
				var method = info as MethodInfo;
				command.WithAction( ( e ) => method.Invoke( null, e ) );
			}
		}


		[AttributeUsage( AttributeTargets.Property, Inherited = false, AllowMultiple = false )]
		public class VarAttribute : CmdAttribute
		{
			/// <summary> TODO: Actually make this work </summary>
			public bool Saved { get; set; }
			public bool IsReadOnly { get; set; }

			public VarAttribute( string name ) : base( name ) { }

			protected override void OnCreate( ref Command command, MemberInfo info )
			{
				var property = info as PropertyInfo;
				var name = command.Name;

				// if ( Saved )
				// Set the propertys value when we create the command.
				// Only static propertys should be allowed to be saved.

				command.WithAction(
					( parameters ) =>
				 	{
						 if ( !IsReadOnly && parameters is not null && parameters.Length > 0 )
						 {
							 property.SetValue( null, parameters[0] );
							 Debugging.Log.Info( $"{name} is now {property.GetValue( null )}" );

							 if ( Saved )
								 SaveValue();
						 }
						 else
						 {
							 Debugging.Log.Info( $"{name} = {property.GetValue( null )}" );
						 }
				 	} );
			}

			protected override StringBuilder BuildHelpMessage( MemberInfo info, StringBuilder builder )
			{
				if ( info is not PropertyInfo property )
					return builder;

				builder.Append( $" {property.PropertyType.Name} " );

				return builder;
			}


			protected virtual void SaveValue()
			{

			}
		}
	}
}
