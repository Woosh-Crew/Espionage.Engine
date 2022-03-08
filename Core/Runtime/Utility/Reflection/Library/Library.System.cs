using System;
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
			Callback.Register( value );
			return Database[value.GetType()];
		}

		/// <summary>
		/// Cleans up ILibrary object, removes it from instance
		/// callback database so the garbage collector picks it up.
		/// </summary>
		/// <param name="value"></param>
		public static void Unregister( ILibrary value )
		{
			Callback.Unregister( value );
		}

		/// <summary>
		/// Constructs ILibrary, if it it has a custom constructor
		/// it'll use that to create the ILibrary
		/// </summary>
		internal static ILibrary Construct( Library library )
		{
			if ( library is null )
			{
				Debugging.Log.Error( "Can't construct, Library is null" );
				return null;
			}

			if ( !library.Spawnable )
			{
				Debugging.Log.Error( $"{library.Name} is not spawnable. Set Spawnable to true in classes meta." );
				return null;
			}

			if ( library.Components.TryGet<ConstructorAttribute>( out var constructor ) )
			{
				return constructor.Invoke() as ILibrary;
			}

			if ( !library.Class.IsAbstract )
			{
				return Activator.CreateInstance( library.Class ) as ILibrary;
			}

			Debugging.Log.Error( $"Can't construct {library.Name}, is abstract and doesn't have constructor predefined." );
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

			using ( Debugging.Stopwatch( "Library Initialized", 0 ) )
			{
				// Select all types where ILibrary exists or if it has the correct attribute
				for ( var assemblyIndex = 0; assemblyIndex < AppDomain.CurrentDomain.GetAssemblies().Length; assemblyIndex++ )
				{
					var assembly = AppDomain.CurrentDomain.GetAssemblies()[assemblyIndex];

					if ( assembly != typeof( Library ).Assembly )
					{
						// Some fully awesome code  - maybe stupid? i dont care its awesome
						if ( assembly.GetReferencedAssemblies().All( e => e.Name != typeof( Library ).Assembly.GetName().Name ) )
						{
							continue;
						}
					}

					var types = assembly.GetTypes();
					for ( var typeIndex = 0; typeIndex < types.Length; typeIndex++ )
					{
						var type = types[typeIndex];

						if ( type.HasInterface<ILibrary>() || type.IsDefined( typeof( LibraryAttribute ), true ) )
						{
							Database.Add( CreateRecord( type ) );
						}
					}
				}
			}

			Initialized = true;
			Callback.Run( "library.ready" );
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
	}
}
