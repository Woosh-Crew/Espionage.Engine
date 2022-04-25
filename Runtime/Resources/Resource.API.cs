using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	public partial class Resource
	{
		public static T Load<T>( Pathing path, bool persistant = false ) where T : class, IResource, new()
		{
			Library library = typeof( T );

			// Apply shorthand, if path doesn't have one
			if ( !path.IsValid() && library.Components.TryGet<PathAttribute>( out var attribute ) )
			{
				path = $"{attribute.ShortHand}://" + path;
			}

			path = path.Virtual().Normalise();
			
			Debugging.Log.Info( $"Loading Resource [{library.Title}] at Path [{path}]" );

			if ( Registered[path]?.Resource != null )
			{
				var asset = Registered[path];

				asset.Resource.Load();
				asset.Resource.Persistant ^= persistant;

				return asset.Resource as T;
			}

			if ( path.Exists() )
			{
				var asset = new T { Persistant = persistant, Identifier = path.Hash() };

				Registered.Fill( path );
				Registered.Add( asset );

				asset.Setup( path );
				asset.Load();

				return asset;
			}

			// Either Load Error Model, or nothing if not found.
			Debugging.Log.Error( $"{library.Title} Path [{path.Output}], couldn't be found." );

			// Load default resource, if its not there
			if ( library.Components.TryGet( out FileAttribute files ) && !files.Fallback.IsEmpty() )
			{
				Debugging.Log.Info( "Loading Fallback" );

				Pathing fallback = files.Fallback;
				fallback = fallback.Virtual().Normalise();

				return !fallback.Exists() ? null : Load<T>( fallback, true );
			}

			return null;
		}

		public static void Unload( Pathing path )
		{
			path = path.Virtual();

			var resource = Registered[path];

			if ( resource == null )
			{
				Debugging.Log.Error( $"Resource [{path}] couldn't be found" );
				return;
			}

			Registered.Remove( resource );
		}

		public static void Unload( IResource resource )
		{
			Assert.IsNull( resource );

			if ( resource!.Unload() && !resource.Persistant )
			{
				Registered.Remove( Registered[resource.Identifier] );
			}
		}
	}
}
