using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace Espionage.Engine
{
	public static partial class Console
	{
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
				var command = new Command()
				{
					Name = this.Name,
					Help = this.Help,
					Layer = this.Layer,
					Info = info,
				};

				command.WithAction( ( e ) => method.Invoke( null, e ) );

				return command;


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

				var command = new Command()
				{
					Name = this.Name,
					Help = this.Help,
					Layer = this.Layer,
					Info = info,
				};

				command.WithAction(
					( parameters ) =>
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
				 	} );

				return command;
			}
		}
	}
}
