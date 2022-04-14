using System;
using System.Collections.Generic;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public partial class Library
	{
		/// <summary>
		/// All static Properties and Functions can be found in the Globals Library
		/// database index. It is here for easy viewing
		/// </summary>
		public static Library Global => Database[typeof( Global )];

		/// <summary>
		/// Database for Library Records. Allows the access of all records.
		/// Use extension methods to add functionality to database access.
		/// </summary>
		public static Libraries Database { get; private set; }

		/// <summary>
		/// Registers the target object with the Library.
		/// Which allows it to receive instance callbacks and
		/// returns its library instance.
		/// </summary>
		public static Library Register( ILibrary value )
		{
			Library lib = value.GetType();
			Assert.IsNull( lib );

			if ( IsSingleton( lib ) )
			{
				if ( Singletons.ContainsKey( lib.Info ) )
				{
					Debugging.Log.Error( $"You're trying to register another Singleton [{lib.Name}]" );
					return null;
				}

				Singletons.Add( lib.Info, value );
			}

			Callback.Register( value );

			return lib;
		}

		/// <summary>
		/// Cleans up ILibrary object, removes it from instance
		/// callback database so the garbage collector picks it up.
		/// </summary>
		/// <param name="value"></param>
		public static void Unregister( ILibrary value )
		{
			// Check if Library is Singleton
			if ( IsSingleton( value.ClassInfo ) )
			{
				Singletons.Remove( value.GetType() );
			}

			Callback.Unregister( value );
		}

		/// <summary>
		/// Checks if the lib has a singleton component,
		/// and an instance of it somewhere.
		/// </summary>
		public static bool IsSingleton( Library lib )
		{
			return lib.Components.Has<SingletonAttribute>() && !lib.Info.HasInterface( typeof( IComponent ) );
		}

		/// <summary>
		/// Constructs ILibrary, if it it has a custom constructor
		/// it'll use that to create the ILibrary
		/// </summary>
		public static ILibrary Create( Library library )
		{
			if ( library == null )
			{
				Debugging.Log.Error( "Can't construct, Library is null" );
				return null;
			}

			if ( library.Spawnable )
			{
				return IsSingleton( library ) && Singletons.ContainsKey( library.Info ) ? Singletons[library.Info] : Construct( library );
			}

			Debugging.Log.Error( $"{library.Name} is not spawnable. Set Spawnable to true in classes meta." );
			return null;
		}

		/// <summary> <inheritdoc cref="Create"/> and returns T </summary>
		public static T Create<T>( Library lib = null ) where T : ILibrary
		{
			lib ??= typeof( T );
			return (T)Create( lib );
		}

		private static ILibrary Construct( Library library )
		{
			// Check if we have a custom Constructor
			if ( library.Components.TryGet<ConstructorAttribute>( out var constructor ) )
			{
				return constructor.Invoke() as ILibrary;
			}

			if ( !library.Info.IsAbstract )
			{
				return Activator.CreateInstance( library.Info ) as ILibrary;
			}

			Debugging.Log.Error( $"Can't construct {library.Name}, is abstract and doesn't have constructor predefined." );
			return null;
		}

		//
		// Manager
		//

		private static Dictionary<Type, ILibrary> Singletons { get; } = new();

		static Library()
		{
			Database = new();

			using ( Debugging.Stopwatch( "Library Initialized" ) )
			{
				Database.Add( new Library( typeof( Global ) ) );
				Database.Add( AppDomain.CurrentDomain );
			}
		}

		internal static bool IsValid( Type type )
		{
			return type.HasInterface( typeof( ILibrary ) ) || type.IsDefined( typeof( LibraryAttribute ), true );
		}
	}
}
