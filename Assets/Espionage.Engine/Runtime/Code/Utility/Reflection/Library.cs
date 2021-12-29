using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	[Manager( nameof( Cache ) )]
	public partial class Library
	{
		//
		// Exposed API
		//

		/// <summary> Helpers to grab library records </summary>
		public static LibraryAccessor Accessor => new LibraryAccessor();

		/// <summary> Helpers to create entities from library records. </summary>
		public static LibraryCreator Creator => new LibraryCreator();

		/// <summary> Every library record. </summary>
		public static IEnumerable<Library> GetAll()
		{
			lock ( _records )
			{
				return _records;
			}
		}

		/// <summary> Constructs ILibrary, if it it has a custom constructor
		/// itll use that to create the ILibrary </summary>
		public static ILibrary Construct( Library library )
		{
			if ( library is null )
			{
				Debug.LogError( "Can't construct, Library is null" );
				return null;
			}

			if ( library.Constructor is not null )
				return library.Constructor.Invoke( null, new object[] { library.Owner } ) as ILibrary;

			return Activator.CreateInstance( library.Owner ) as ILibrary;
		}

		//
		// Manager
		//

		private static void Cache()
		{
			using ( Debugging.Stopwatch( "Library Initialized" ) )
			{
				// Select all types where ILibrary exsists or if it has the correct attribute
				var types = AppDomain.CurrentDomain.GetAssemblies()
									.SelectMany( e => e.GetTypes()
									.Where( e => !e.IsAbstract && (e.IsDefined( typeof( LibraryAttribute ) ) || e.GetInterfaces().Contains( typeof( ILibrary ) )) ) );

				foreach ( var item in types )
					AddRecord( CreateRecord( item ) );
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
				Title = type.Name,
				Owner = type,
			};

			// Generate the ID, so we can spawn it at runtime
			record.Id = GenerateID( record.Name );

			// Get the constructor, incase it happens to have a custom one
			record.Constructor = GetConstructor( type );

			return record;
		}

		private static MethodInfo GetConstructor( Type type )
		{
			// Check if there is a constructor | INTERNAL
			if ( type.IsDefined( typeof( ConstructorAttribute ), true ) )
			{
				var attribute = type.GetCustomAttribute<ConstructorAttribute>( true );
				var method = type.GetMethod( attribute.Constructor, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );

				return method;
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

		private static void AddRecord( Library library )
		{
			// Check if we already have that name
			if ( _records.Any( e => e.Name == library.Name ) )
				throw new Exception( $"Library cache already contains key: {library.Name}" );

			// Check if we already contain that type
			if ( _records.Any( e => e.Owner == library.Owner ) )
				throw new Exception( $"Library cache already contains type: {library.Owner.FullName}" );

			// Check if we already contain that Id, this will fuckup networking
			if ( _records.Any( e => e.Id == library.Id ) )
				throw new Exception( $"Library cache already contains GUID: {library.Id}" );

			_records.Add( library );
		}

		private static List<Library> _records = new List<Library>();

		//
		// Instance
		//

		public string Name { get; internal set; }
		public string Title { get; internal set; }
		public string Description { get; internal set; }

		public Guid Id { get; internal set; }
		public Type Owner { get; internal set; }

		internal MethodInfo Constructor { get; set; }
	}
}
