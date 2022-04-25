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

			var resource = Find( path.Virtual().Normalise() );
			return resource != null ? Load<T>( resource, persistant ) : Fallback<T>();
		}

		public static T Load<T>( Resource resource, bool persistant = false ) where T : class, IAsset, new()
		{
			Library library = typeof( T );

			Debugging.Log.Info( $"Loading Resource [{library.Title}] at Path [{resource.Path}]" );

			resource.Source ??= resource.Create<T>();
			resource.Source.Load();
			resource.Persistant ^= persistant;

			return resource.Source as T;
		}

		public static T Fallback<T>() where T : class, IAsset, new()
		{
			Library library = typeof( T );

			// Load default resource, if its not there
			if ( !library.Components.TryGet( out FileAttribute files ) || files.Fallback.IsEmpty() )
			{
				return null;
			}

			Debugging.Log.Error( $"Loading fallback for [{library.Title}]" );

			Pathing fallback = files.Fallback;
			fallback = fallback.Virtual().Normalise();

			return !fallback.Exists() ? null : Load<T>( fallback, true );
		}

		public static Resource Find( Pathing path )
		{
			path = path.Virtual().Normalise();
			return Registered[path] != null ? Registered[path] : (path.Exists() ? Registered.Fill( path ) : null);
		}
	}
}
