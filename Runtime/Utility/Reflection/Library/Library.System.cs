using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public partial class Library
	{
		/// <summary>
		/// Registers the target object with the Library.
		/// Which allows it to receive instance callbacks and
		/// returns its library instance.
		/// </summary>
		public static Library Register( ILibrary value )
		{
			Library lib = value.GetType();

			if ( lib == null )
			{
				Debugging.Log.Error( $"[FATAL] {value} was null in library database." );
				return null;
			}

			// Check if Library is Singleton & not a component.
			if ( lib.Components.Has<SingletonAttribute>() && !lib.Info.HasInterface( typeof( IComponent ) ) )
			{
				if ( Singletons.ContainsKey( lib.Info ) )
				{
					Debugging.Log.Error( $"You are trying to register another Singleton? How???? -- [{lib.Name}]" );
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
			if ( value.ClassInfo?.Components.Has<SingletonAttribute>() ?? false )
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
			return lib.Components.Has<SingletonAttribute>() && Singletons.ContainsKey( lib.Info );
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
				return IsSingleton( library ) ? Singletons[library.Info] : Construct( library );
			}

			Debugging.Log.Error( $"{library.Name} is not spawnable. Set Spawnable to true in classes meta." );
			return null;
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

		static Library()
		{
			Database = new() { new Library( typeof( Global ) ) };

			using ( Debugging.Stopwatch( "Library Initialized" ) )
			{
				var main = typeof( Library ).Assembly;
				Database.Add( main );

				// Select all types where ILibrary exists or if it has the correct attribute
				foreach ( var assembly in AppDomain.CurrentDomain.GetAssemblies() )
				{
					if ( assembly != main && assembly.GetReferencedAssemblies().Any( e => e.FullName == main.FullName ) )
					{
						Database.Add( assembly );
					}
				}
			}
		}

		internal static bool IsValid( Type type )
		{
			return type.HasInterface( typeof( ILibrary ) ) || type.IsDefined( typeof( LibraryAttribute ), true );
		}

		//
		// Singletons
		//

		private static Dictionary<Type, ILibrary> Singletons { get; } = new();
	}
}
