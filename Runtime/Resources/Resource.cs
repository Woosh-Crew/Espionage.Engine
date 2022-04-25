using System.Collections.Generic;
using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	public sealed class Resource
	{
		public Resource( Pathing path )
		{
			Path = path;
			Identifier = path.Hash();
		}

		public override int GetHashCode()
		{
			return Identifier;
		}

		public override string ToString()
		{
			return $"loaded:[{IsLoaded}] path:[{Path}]";
		}

		// State

		public bool Persistant { get; set; }
		public bool IsLoaded => Source != null;

		// References

		public IAsset Source { get; private set; }
		public List<IAsset> Instances { get; private set; }

		// Identification

		public int Identifier { get; }
		public Pathing Path { get; }

		// Management

		public T Create<T>() where T : class, IAsset, new()
		{
			Assert.IsTrue( Source != null );

			Source = new T();
			Source.Resource = this;
			Source.Setup( Path );

			return Source as T;
		}

		public T Load<T>( bool persistant = false ) where T : class, IAsset, new()
		{
			Persistant ^= persistant;

			Library library = typeof( T );

			if ( !IsLoaded )
			{
				Debugging.Log.Info( $"Loading {library.Title} at Path [{Path}]" );

				Instances = new();
				Source = Create<T>();
				Source.Load();
			}

			var instance = Source.Clone();

			if ( instance == null )
			{
				Debugging.Log.Error( $"Can't load [{library.Title}]" );
				return null;
			}

			Instances.Add( instance );
			instance.Resource = this;

			return instance as T;
		}

		public void Unload( bool forcefully )
		{
			if ( !IsLoaded )
			{
				// Nothing was loaded
				return;
			}

			foreach ( var instance in Instances )
			{
				if ( instance == Source )
				{
					continue;
				}

				instance.Delete();
			}

			Instances.Clear();

			if ( forcefully || !Persistant )
			{
				Debugging.Log.Info( $"Unloading {Source.ClassInfo.Title} [{Path}]" );

				Source.Unload();
				Source.Delete();
				Source = null;
			}
		}
	}
}
