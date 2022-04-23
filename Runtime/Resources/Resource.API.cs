namespace Espionage.Engine.Resources
{
	public partial class Resource
	{
		public static T Load<T>( string path, bool persistant = false ) where T : class, IResource, new()
		{
			Library library = typeof( T );
			path = Files.Pathing.Absolute( path );

			// Apply shorthand, if path doesn't have one
			if ( !Files.Pathing.Valid( path ) && library.Components.TryGet<PathAttribute>( out var pathing ) )
			{
				path = $"{pathing.ShortHand}://" + path;
			}

			if ( Registered[path]?.Resource != null )
			{
				var asset = Registered[path];

				asset.Resource.Load();
				asset.Resource.Persistant ^= persistant;

				return asset.Resource as T;
			}

			if ( Files.Pathing.Exists( path ) )
			{
				var asset = new T { Persistant = persistant, Identifier = path.Hash() };

				Registered.Fill( path );
				Registered.Add( asset );

				asset.Setup( path );
				asset.Load();

				return asset;
			}

			// Either Load Error Model, or nothing if not found.
			Debugging.Log.Error( $"{library.Title} Path [{path}], couldn't be found." );

			// Load default resource, if its not there
			if ( library.Components.TryGet( out FileAttribute files ) && Registered[files.Fallback] != null )
			{
				Debugging.Log.Info( "Loading Fallback" );
				return Load<T>( files.Fallback, true );
			}

			return null;
		}

		public static void Unload( string path )
		{
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
