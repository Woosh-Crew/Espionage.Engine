using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;
using UnityEngine;
using Random = System.Random;

namespace Espionage.Engine
{
	[Manager( nameof( Cache ), Layer = Layer.Editor | Layer.Runtime, Order = -10 )]
	public partial class Library
	{
		/// <summary> Constructs ILibrary, if it it has a custom constructor
		/// it'll use that to create the ILibrary </summary>
		internal static object Construct( Library library )
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
				return constructor.Invoke();
			}

			if ( !library.Class.IsAbstract )
			{
				return Activator.CreateInstance( library.Class );
			}

			Debugging.Log.Error( $"Can't construct {library.Name}, is abstract and doesn't have constructor predefined." );
			return null;
		}

		//
		// Manager
		//

		private static void Cache()
		{
			Database ??= new InternalDatabase();
			Database.Clear();

			using ( Debugging.Stopwatch( "Library Initialized" ) )
			{
				// Select all types where ILibrary exists or if it has the correct attribute
				var types = AppDomain.CurrentDomain.GetAssemblies()
					.Where( Utility.IgnoreIfNotUserGeneratedAssembly )
					.SelectMany( e => e.GetTypes()
						.Where( ( type ) =>
						{
							if ( type.IsDefined( typeof( Skip ) ) )
							{
								return false;
							}

							return type.IsDefined( typeof( LibraryAttribute ) ) || type.HasInterface<ILibrary>();
						} ) );

				foreach ( var item in types )
				{
					Database.Add( CreateRecord( item ) );
				}
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
			record ??= new Library( type );

			return record;
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

			return new Guid( guid );
		}
	}
}
