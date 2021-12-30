using System;
using System.Collections.Generic;
using System.Reflection;

using Espionage.Engine.Internal;
using Debug = UnityEngine.Debug;

namespace Espionage.Engine
{
	public static partial class Console
	{
		[AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
		public class CmdAttribute : Attribute, ICommandCreator
		{
			readonly string[] names;

			public string[] Names => names;
			public string Help { get; set; }

			public CmdAttribute( params string[] names )
			{
				this.names = names;
			}

			public Command[] Create( MemberInfo info )
			{
				List<Command> commands = new List<Command>();

				foreach ( var item in Names )
				{
					var command = new Command()
					{
						Name = item,
						Help = this.Help,
						Info = info,
					};

					OnCreate( ref command, info );
					commands.Add( command );
				}

				return commands.ToArray();
			}

			protected virtual void OnCreate( ref Command command, MemberInfo info )
			{
				var method = info as MethodInfo;
				command.WithAction( ( e ) => method.Invoke( null, e ) );
			}
		}


		[AttributeUsage( AttributeTargets.Property, Inherited = false, AllowMultiple = false )]
		public sealed class VarAttribute : CmdAttribute
		{
			public bool IsReadOnly { get; set; }

			public VarAttribute( string name ) : base( name ) { }

			protected override void OnCreate( ref Command command, MemberInfo info )
			{
				var property = info as PropertyInfo;
				var name = command.Name;

				command.WithAction(
					( parameters ) =>
				 	{
						 if ( !IsReadOnly && parameters is not null && parameters.Length > 0 )
						 {
							 property.SetValue( null, parameters[0] );
							 Debug.Log( $"{name} is now {property.GetValue( null )}" );
						 }
						 else
						 {
							 Debug.Log( $"{name} = {property.GetValue( null )}" );
						 }
				 	} );
			}
		}
	}
}
