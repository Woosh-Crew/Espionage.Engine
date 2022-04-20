using System.Reflection;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public sealed class Function : IMember<MethodInfo>, ILibrary
	{
		public Library Owner { get; set; }
		public Library ClassInfo => Library.Database[typeof( Function )];

		public Components<Function> Components { get; }
		public MethodInfo Info { get; }

		internal Function( MethodInfo info, string name = null )
		{
			Info = info;
			Name = name.IsEmpty( info.Name.ToProgrammerCase() );

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

			Title = Title.IsEmpty( info.Name.ToTitleCase() );
		}

		public override string ToString()
		{
			return $"Function:[{Name}/{Owner.Name}]";
		}

		public int Identifier { get; set; }
		public string Name { get; }
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
