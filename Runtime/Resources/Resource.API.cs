namespace Espionage.Engine.Resources
{
	public partial class Resource
	{
		public static T Load<T>( string path, bool persistant = false ) where T : class, IResource, new()
		{
			Library library = typeof( T );

			if ( !Files.Pathing.Valid( path ) && library.Components.TryGet<PathAttribute>( out var pathing ) )
			{
				path = $"{pathing.ShortHand}://" + path;
			}

			path = Files.Pathing.Absolute( path );

			if ( Registered[path] != null )
			{
				var asset = Registered[path];
				asset.Resource.Load();
				return asset as T;
			}

			if ( Files.Pathing.Exists( path ) )
			{
				var model = new T { Identifier = path.Hash(), Persistant = persistant };
				Registered.Add( model );

				model.Setup( path );
				model.Load();

				return model;
			}

			// Either Load Error Model, or nothing if not found.
			Debugging.Log.Error( $"{library.Title} Path [{path}], couldn't be found." );

			// Load default resource, if its not there
			if ( library.ClassInfo.Components.TryGet( out FileAttribute files ) && !files.Fallback.IsEmpty() && Files.Pathing.Exists( files.Fallback ) )
			{
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

			Unload( resource.Resource );
		}

		public static void Unload( IResource resource )
		{
			Assert.IsNull( resource );

			if ( resource!.Unload() && !resource.Persistant )
			{
				Registered.Remove( resource );
			}
		}
	}
}
