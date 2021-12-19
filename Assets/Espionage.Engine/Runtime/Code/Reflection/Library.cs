using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mirror;
using UnityEngine;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	public partial class Library
	{
		//
		// Exposed API
		//

		/// <summary> Helpers to grab library records </summary>
		public static LibraryAccessor Accessor => new LibraryAccessor();

		/// <summary> Helpers to create entities from library records. </summary>
		public static LibraryCreator Creator => new LibraryCreator();

		public static IEnumerable<Library> GetAll() => _records.Values;

		//
		// Manager
		//

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterAssembliesLoaded )]
		private static void Cache()
		{
			// Select all types where ILibrary exsists
			var types = AppDomain.CurrentDomain.GetAssemblies()
								.SelectMany( e => e.GetTypes()
								.Where( e => !e.IsAbstract && e.GetInterfaces().Contains( typeof( ILibrary ) ) ) );

			foreach ( var item in types )
				AddRecord( CreateRecord( item ) );
		}

		private static Library CreateRecord( Type type )
		{
			Library record;

			// If we have meta present, use it
			if ( type.IsDefined( typeof( LibraryAttribute ), false ) )
			{
				var attribute = type.GetCustomAttribute<LibraryAttribute>();
				record = attribute.CreateRecord( type );
			}

			// If not just use type defaults
			else
			{
				record = new Library()
				{
					Name = type.FullName,
					Title = type.Name,
					Owner = type,
				};
			}

			// Generate the ID, so we can spawn it at runtime
			record.Id = GenerateID( record.Name );

			return record;
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
			if ( _records.Any( e => e.Value.Name == library.Name ) )
				throw new Exception( $"Library cache already contains key: {library.Name}" );

			// Check if we already contain that type
			if ( _records.Any( e => e.Value.Owner == library.Owner ) || _records.ContainsKey( library.Owner ) )
				throw new Exception( $"Library cache already contains type: {library.Owner.FullName}" );

			// Check if we already contain that Id, this will fuckup networking
			if ( _records.Any( e => e.Value.Id == library.Id ) )
				throw new Exception( $"Library cache already contains GUID: {library.Id}" );

			_records.Add( library.Owner, library );
		}

		private static Dictionary<Type, Library> _records = new Dictionary<Type, Library>();

		//
		// Entity / Object Creation
		//

		//
		// Instance
		//

		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		// Mirror uses GUIDs?
		public Guid Id { get; set; }
		public Type Owner { get; set; }
	}
}
