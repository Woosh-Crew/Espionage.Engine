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

			if ( Database[path] != null )
			{
				var asset = Database[path];
				asset.Load();
				return asset as T;
			}

			if ( Files.Pathing.Exists( path ) )
			{
				var model = new T { Identifier = path.Hash(), Persistant = persistant };
				Database.Add( model );

				model.Setup( path );
				model.Load();

				return model;
			}

			// Either Load Error Model, or nothing if not found.
			Debugging.Log.Error( $"{library.Title} Path [{path}], couldn't be found." );
			return null;
		}

		public static void Unload( string path )
		{
			var resource = Database[path];

			if ( resource == null )
			{
				Debugging.Log.Error( $"Resource [{path}] couldn't be found" );
				return;
			}

			Unload( resource );
		}

		public static void Unload( IResource resource )
		{
			Assert.IsNull( resource );

			if ( resource!.Unload() && !resource.Persistant )
			{
				Database.Remove( resource );
			}
		}
	}
}
