using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	/// <summary> Espionage.Engines string based Reflection System </summary>
	[Serializable] // Instance Serialization
	[Manager( nameof( Cache ), Layer = Layer.Editor | Layer.Runtime, Order = -10 )]
	public partial class Library
	{
		//
		// Public API
		//

		/// <summary> Database for library records </summary>
		public static IDatabase<Library> Database => _database;

		/// <summary> Attribute that skips the attached class from generating a library reference </summary>
		[AttributeUsage( AttributeTargets.Class )]
		public class Skip : Attribute { }


		/// <summary> Attribute that allows the definition of a custom constructor </summary>
		[AttributeUsage( AttributeTargets.Class, Inherited = true )]
		public sealed class Constructor : Attribute
		{
			/// <param name="constructor"> Method should return ILibrary </param>
			public Constructor( string constructor )
			{
				this.constructor = constructor;
			}

			private string constructor;
			public string Target => constructor;
		}

		//
		// Database
		//

		[Serializable] // Database Serialization
		private class internal_Database : IDatabase<Library>
		{
#if UNITY_STANDALONE || UNITY_EDITOR
			[UnityEngine.SerializeField]
#endif
			private List<Library> records = new List<Library>();
			public IEnumerable<Library> All => records;

			public void Add( Library item )
			{
				// Check if we already have that name
				if ( records.Any( e => e.Name == item.Name ) )
					throw new Exception( $"Library cache already contains key: {item.Name}" );

				// Check if we already contain that Id, this will fuckup networking
				if ( records.Any( e => e.Id == item.Id ) )
					throw new Exception( $"Library cache already contains GUID: {item.Id}" );

				records.Add( item );
			}

			public void Clear()
			{
				records.Clear();
			}

			public bool Contains( Library item )
			{
				return records.Contains( item );
			}

			public void Remove( Library item )
			{
				records.Remove( item );
			}

			public void Replace( Library oldItem, Library newItem )
			{
				if ( !Contains( oldItem ) )
				{
					Debugging.Log.Warning( $"Library doesnt contain item {oldItem}" );
					return;
				}

				var index = records.IndexOf( oldItem );
				records[index] = newItem;
			}


			// Use Unitys shitty json serialization
#if UNITY_STANDALONE || UNITY_EDITOR

			public string Serialize()
			{
				var json = UnityEngine.JsonUtility.ToJson( this, true );
				return json;
			}

#endif
		}

		private static IDatabase<Library> _database;

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

		//
		// Instance
		//

		public string Name;
		public string Title;
		public string Description;
		public string Icon;

		public int Order;

		// Owner

		public Library WithOwner( Type type )
		{
			Owner = type;
			return this;
		}

		[NonSerialized]
		public Type Owner;

		// GUID

		public Library WithId( string name )
		{
			Id = GenerateID( name );
			return this;
		}

		[NonSerialized]
		public Guid Id;

		// Construtor

		public Library WithConstructor( ConstructorRef constructor )
		{
			_constructor = constructor;
			return this;
		}

		[NonSerialized]
		private ConstructorRef _constructor;
	}
}
