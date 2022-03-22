using System.Reflection;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public sealed class Function : IMember
	{
		public Library Owner { get; }

		public MethodInfo Info { get; }
		public Components<Function> Components { get; }

		internal Function( Library owner, MethodInfo info, string name )
		{
			Owner = owner;
			Info = info;

			Name = name;
			Title = info.Name;
			Group = owner.Title;

			// Components
			Components = new( this );

			// This is really expensive (6ms)...
			// Get Components attached to type
			foreach ( var item in Info.GetCustomAttributes() )
			{
				if ( item is IComponent<Function> property )
				{
					Components.Add( property );
				}
			}
		}

		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }

		public bool IsStatic => Info.IsStatic;

		//
		// Invokers
		//

		private object[] GetDefaultArgs( object[] args )
		{
			var parameters = Info.GetParameters();

			if ( parameters.Length == 0 )
			{
				return null;
			}

			var finalArgs = new object[parameters.Length];

			for ( var i = 0; i < parameters.Length; i++ )
			{
				finalArgs[i] = args?[i] == null || i >= args.Length ? parameters[i].DefaultValue : args[i];
			}

			return finalArgs;
		}

		public object Invoke( object target )
		{
			return Info.Invoke( target, GetDefaultArgs( null ) );
		}

		public T Invoke<T>( object target )
		{
			return (T)Info.Invoke( target, GetDefaultArgs( null ) );
		}

		public object Invoke( object target, params object[] parameters )
		{
			return Info.Invoke( target, GetDefaultArgs( parameters ) );
		}

		public T Invoke<T>( object target, params object[] parameters )
		{
			return (T)Info.Invoke( target, GetDefaultArgs( parameters ) );
		}
	}
}
