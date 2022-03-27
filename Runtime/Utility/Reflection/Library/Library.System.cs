using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
			var lib = Database[value.GetType()];

			if ( lib == null )
			{
				Dev.Log.Error( $"[FATAL] {value} was null in library database." );
				return null;
			}

			// Check if Library is Singleton
			if ( lib.Components.Has<SingletonAttribute>() )
			{
				if ( Singletons.ContainsKey( lib.Info ) )
				{
					Dev.Log.Error( $"You are trying to register another Singleton? How???? -- [{lib.Name}]" );
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
		/// Constructs ILibrary, if it it has a custom constructor
		/// it'll use that to create the ILibrary
		/// </summary>
		public static ILibrary Create( Library library )
		{
			if ( library is null )
			{
				Dev.Log.Error( "Can't construct, Library is null" );
				return null;
			}

			if ( !library.Spawnable )
			{
				Dev.Log.Error( $"{library.Name} is not spawnable. Set Spawnable to true in classes meta." );
				return null;
			}

			// If we are a singleton, Check if an instance already exists 
			if ( library.Components.Has<SingletonAttribute>() )
			{
				if ( Singletons.ContainsKey( library.Info ) )
				{
					return Singletons[library.Info];
				}

				var newSingleton = Construct( library );
				return newSingleton;
			}

			return Construct( library );
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

			Dev.Log.Error( $"Can't construct {library.Name}, is abstract and doesn't have constructor predefined." );
			return null;
		}

		//
		// Manager
		//

		public static bool Initialized { get; private set; }

		public static void Initialize()
		{
			if ( Initialized )
			{
				return;
			}

			Database = new InternalDatabase();
			Database.Add( new( typeof( Global ) ) );

			using ( Dev.Stopwatch( "Library Initialized" ) )
			{
				var main = typeof( Library ).Assembly;
				AddAssembly( main );

				var assemblies = AppDomain.CurrentDomain.GetAssemblies();

				// Select all types where ILibrary exists or if it has the correct attribute
				for ( var assemblyIndex = 0; assemblyIndex < assemblies.Length; assemblyIndex++ )
				{
					var assembly = assemblies[assemblyIndex];

					if ( assembly != main && assembly.GetReferencedAssemblies().Any( e => e.FullName == main.FullName ) )
					{
						AddAssembly( assembly );
					}
				}
			}

			Initialized = true;
			Callback.Run( "library.ready" );
		}

		private static void AddAssembly( Assembly assembly )
		{
			var types = assembly.GetTypes();
			for ( var typeIndex = 0; typeIndex < types.Length; typeIndex++ )
			{
				var type = types[typeIndex];

				if ( IsValid( type ) )
				{
					Database.Add( CreateRecord( type ) );
				}
			}
		}

		internal static bool IsValid( Type type )
		{
			return type.HasInterface( typeof( ILibrary ) ) || type.IsDefined( typeof( LibraryAttribute ), true );
		}

		private static Library CreateRecord( Type type )
		{
			if ( !type.IsDefined( typeof( LibraryAttribute ), false ) )
			{
				return new( type );
			}

			// If we have meta present, use it
			var attribute = type.GetCustomAttribute<LibraryAttribute>();
			return attribute.CreateRecord( type );
		}

		private static Guid GenerateID( string name )
		{
			if ( string.IsNullOrEmpty( name ) )
			{
				return default;
			}

			var random = new Random( name.GetHashCode( StringComparison.InvariantCultureIgnoreCase ) );
			var guid = new byte[16];
			random.NextBytes( guid );

			return new( guid );
		}

		//
		// Singletons
		//

		private static Dictionary<Type, ILibrary> Singletons { get; } = new();
	}
}
