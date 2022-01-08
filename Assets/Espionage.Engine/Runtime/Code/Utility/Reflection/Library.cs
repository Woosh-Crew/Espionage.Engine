using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	[Manager( nameof( Cache ), Layer = Layer.Editor | Layer.Runtime, Order = -10 ), Serializable]
	public partial class Library
	{
		private class internal_Database : IDatabase<Library>
		{
			private static List<Library> _records = new List<Library>();
			public IEnumerable<Library> All => _records;

			public void Add( Library item )
			{
				// Check if we already have that name
				if ( _records.Any( e => e.Name == item.Name ) )
					throw new Exception( $"Library cache already contains key: {item.Name}" );

				// Check if we already contain that type
				if ( _records.Any( e => e.Owner == item.Owner ) )
					throw new Exception( $"Library cache already contains type: {item.Owner.FullName}" );

				// Check if we already contain that Id, this will fuckup networking
				if ( _records.Any( e => e.Id == item.Id ) )
					throw new Exception( $"Library cache already contains GUID: {item.Id}" );

				_records.Add( item );
			}

			public void Clear()
			{
				_records.Clear();
			}

			public void Contains( Library item )
			{
				_records.Contains( item );
			}

			public void Remove( Library item )
			{
				_records.Remove( item );
			}
		}

		//
		// Exposed API
		//

		private static IDatabase<Library> _database;

		/// <summary> Database for library records </summary>
		public static IDatabase<Library> Database => _database;


		/// <summary> Constructs ILibrary, if it it has a custom constructor
		/// itll use that to create the ILibrary </summary>
		public static ILibrary Construct( Library library )
		{
			if ( library is null )
			{
				Debugging.Log.Error( "Can't construct, Library is null" );
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
			_database ??= new internal_Database();
			_database.Clear();

			using ( Debugging.Stopwatch( "Library Initialized" ) )
			{
				// Select all types where ILibrary exsists or if it has the correct attribute
				var types = AppDomain.CurrentDomain.GetAssemblies()
									.SelectMany( e => e.GetTypes()
									.Where( e => !e.IsAbstract && (e.IsDefined( typeof( LibraryAttribute ) ) || e.GetInterfaces().Contains( typeof( ILibrary ) )) ) );

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

		//
		// Instance
		//

		public string Name;
		public string Title;
		public string Description;
		public string Icon;

		public int Order;


		[NonSerialized]
		public Guid Id;

		[NonSerialized]
		public Type Owner;

		[NonSerialized]
		internal MethodInfo Constructor;
	}
}
