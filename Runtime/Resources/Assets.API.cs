using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	public partial class Assets
	{
		public static T Load<T>( Pathing path, bool persistant = false ) where T : class, IAsset, new()
		{
			Library library = typeof( T );

			// Apply shorthand, if path doesn't have one
			if ( !path.IsValid() && library.Components.TryGet<PathAttribute>( out var attribute ) )
			{
				path = $"{attribute.ShortHand}://" + path;
			}

			path = path.Virtual().Normalise();

			Debugging.Log.Info( $"Loading Resource [{library.Title}] at Path [{path}]" );

			// Get resource and load or grab asset from path
			var resource = Registered[path] != null ? Registered[path] : (path.Exists() ? Registered.Fill( path ) : null);

			if ( resource == null )
			{
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

			resource.Asset ??= resource.Create<T>();

			resource.Asset.Load();
			resource.Persistant ^= persistant;

			return resource.Asset as T;
		}
	}
}
