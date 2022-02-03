using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Espionage.Engine.Internal;
using Espionage.Engine.Components;
using Steamworks.Data;
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

		public static string[] IgnoredNamespaces { get; } = new[] { "DiscordAPI" };

		private static void Cache()
		{
			Database ??= new InternalDatabase();
			Database.Clear();

			using ( Debugging.Stopwatch( "Library Initialized", 0 ) )
			{
				// Select all types where ILibrary exists or if it has the correct attribute
				foreach ( var assembly in AppDomain.CurrentDomain.GetAssemblies() )
				{
					if ( !Utility.IgnoreIfNotUserGeneratedAssembly( assembly ) )
					{
						continue;
					}

					foreach ( var type in assembly.GetTypes() )
					{
						if ( !(type.IsAbstract && type.IsSealed || type.HasInterface<ILibrary>()) || IgnoredNamespaces.Any( e => e == type.Namespace ) )
						{
							continue;
						}

						Database.Add( CreateRecord( type ) );
					}
				}
			}

			Callback.Run( "library.ready" );
		}

		private static Library CreateRecord( Type type )
		{
			if ( !type.IsDefined( typeof( LibraryAttribute ), false ) )
			{
				return new Library( type );
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

			return new Guid( guid );
		}
	}
}
