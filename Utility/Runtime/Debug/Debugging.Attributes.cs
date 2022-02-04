using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Espionage.Engine.Internal.Commands;

namespace Espionage.Engine
{
	public static partial class Debugging
	{
		/// <summary>
		/// Add this attribute to a method for it be added to the command database. Then later 
		/// invoked using its name / identifier. Attribute must be attached to a static method.
		/// </summary>
		[AttributeUsage( AttributeTargets.Method, Inherited = false )]
		public class CmdAttribute : Attribute, ICommandCreator
		{
			public string[] Names { get; }

			/// <summary>
			/// What should we print when help is invoked in the console.
			/// </summary>
			public string Help { get; set; }

			/// <summary><inheritdoc cref="CmdAttribute"/></summary>
			/// <param name="names">What is the name of the command to invoke this method.</param>
			public CmdAttribute( params string[] names )
			{
				Names = names;
			}

			public List<Command> Create( MemberInfo info )
			{
				var commands = new List<Command>();

				var helpBuilder = new StringBuilder( Help + " - (" );
				var helpMessage = BuildHelpMessage( info, helpBuilder );
				helpBuilder.Append( " )" );

				foreach ( var item in Names )
				{
					var command = new Command()
					{
						Name = item,
						Help = helpBuilder.ToString(),
						Info = info
					};

					OnCreate( ref command, info );
					commands.Add( command );
				}

				return commands;
			}

			protected virtual StringBuilder BuildHelpMessage( MemberInfo info, StringBuilder builder )
			{
				if ( info is not MethodInfo method )
				{
					return builder;
				}

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
				command.WithAction( ( e ) => method?.Invoke( null, e ) );
			}
		}


		/// <summary>
		/// A Var is basically a <see cref="CmdAttribute"/>, with the command prebuilt when initializing.
		/// This var allows you to change and read a property at any time. You can also serialize the value
		/// for persistence. Attribute must be attached to a static property.
		/// </summary>
		[AttributeUsage( AttributeTargets.Property )]
		public sealed class VarAttribute : CmdAttribute
		{
			/// <summary>
			/// Will this Var be serialized, and saved for use after the application closes?
			/// </summary>
			public bool Saved { get; set; }

			/// <summary>
			/// If True, user wont be able to set the value of the target property.
			/// </summary>
			public bool IsReadOnly { get; set; }

			public VarAttribute( string name ) : base( name ) { }

			protected override void OnCreate( ref Command command, MemberInfo info )
			{
				var property = info as PropertyInfo;
				var name = command.Name;

				// if ( Saved )
				// Set the properties value when we create the command.
				// Only static properties should be allowed to be saved.

				command.WithAction(
					( parameters ) =>
					{
						if ( !IsReadOnly && parameters is not null && parameters.Length > 0 )
						{
							property?.SetValue( null, parameters[0] );
							Log.Info( $"{name} is now {property?.GetValue( null )}" );

							if ( Saved )
							{
								SaveValue();
							}
						}
						else
						{
							Log.Info( $"{name} = {property?.GetValue( null )}" );
						}
					} );
			}

			protected override StringBuilder BuildHelpMessage( MemberInfo info, StringBuilder builder )
			{
				if ( info is not PropertyInfo property )
				{
					return builder;
				}

				builder.Append( $" {property.PropertyType.Name} " );

				return builder;
			}


			private void SaveValue() { }
		}
	}
}
