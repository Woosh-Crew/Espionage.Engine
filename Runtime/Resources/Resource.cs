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

		// State

		public bool Persistant { get; set; }
		public bool IsLoaded => Source != null;

		// References

		public IAsset Source { get; set; }
		public List<IAsset> Instances { get; } = new();

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
			Debugging.Log.Info( $"Loading Resource [{library.Title}] at Path [{Path}]" );

			if ( Source == null )
			{
				Source = Create<T>();
				Source.Load();
			}

			var instance = Source.Clone();
			instance.Resource = this;
			Instances.Add( instance );

			return instance as T;
		}

		public void Unload( bool forcefully )
		{
			foreach ( var instance in Instances )
			{
				instance.Delete();
			}

			if ( forcefully || !Persistant )
			{
				Source.Unload();
			}
		}
	}
}
