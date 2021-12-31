using System;
using System.Reflection;

namespace Espionage.Engine.Internal
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
	}
}
