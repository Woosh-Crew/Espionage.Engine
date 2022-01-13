using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	[Manager( nameof( Cache ), Layer = Layer.Editor | Layer.Runtime, Order = -10 )]
	public partial class Library
	{
		/// <summary> Constructs ILibrary, if it it has a custom constructor
		/// itll use that to create the ILibrary </summary>
		internal static ILibrary Construct( Library library )
		{
			if ( library is null )
			{
				Debugging.Log.Error( "Can't construct, Library is null" );
				return null;
			}

			if ( library._constructor is not null )
				return library._constructor.Invoke( library );

			return Activator.CreateInstance( library.Owner ) as ILibrary;
		}

		//
		// Manager
		//

		private static void Cache()
		{
			_database ??= new internal_Database();
			_database.Clear();

			using ( Debugging.Stopwatch( "Library Initialized" ) )
			{
				// Select all types where ILibrary exsists or if it has the correct attribute
				var types = AppDomain.CurrentDomain.GetAssemblies()
									.SelectMany( e => e.GetTypes()
									.Where( ( e ) =>
									{
										if ( e.IsAbstract )
											return false;

										if ( e.IsDefined( typeof( Skip ) ) )
											return false;

										return (e.IsDefined( typeof( LibraryAttribute ) ) || e.GetInterfaces().Contains( typeof( ILibrary ) ));
									} ) );

				foreach ( var item in types )
					_database.Add( CreateRecord( item ) );
			}
		}

		private static Library CreateRecord( Type type )
		{
			Library record = null;

			// If we have meta present, use it
			if ( type.IsDefined( typeof( LibraryAttribute ), false ) )
			{
				var attribute = type.GetCustomAttribute<LibraryAttribute>();
				record = attribute.CreateRecord( type );
			}

			// If still null just use type defaults
			record ??= new Library()
			{
				Name = type.FullName,
				Order = 0,
				Title = type.Name,
				Owner = type,
			};

			// Generate the ID, so we can spawn it at runtime
			record.Id = GenerateID( record.Name );

			// Get the constructor, incase it happens to have a custom one
			record._constructor = GetConstructor( type );

			return record;
		}

		public delegate ILibrary ConstructorRef( Library type );

		private static ConstructorRef GetConstructor( Type type )
		{
			// Check if there is a constructor | INTERNAL
			if ( type.IsDefined( typeof( Constructor ), true ) )
			{
				var attribute = type.GetCustomAttribute<Constructor>( true );
				var method = type.GetMethod( attribute.Target, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );

				return ( e ) => method.Invoke( null, new object[] { e } ) as ILibrary;
			}

			return null;
		}

		private static Guid GenerateID( string name )
		{
			var random = new Random( name.GetHashCode( StringComparison.InvariantCultureIgnoreCase ) );
			var guid = new byte[16];
			random.NextBytes( guid );

			return new Guid( guid );
		}
	}
}
