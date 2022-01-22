using System;
using System.IO;

namespace Espionage.Engine.Resources
{
	[Title("Resource"), Help( "Allows loading precompiled assets at runtime" )]
	public class Resource<T> : IResource where T : Asset
	{
		public T Asset { get; private set; }

		public Resource( string path )
		{
			if ( !Directory.Exists( path ) )
			{
				Debugging.Log.Error( "Invalid Map Path" );
				throw new DirectoryNotFoundException();
			}
			
			Path = path;
		}
		
		//
		// Resource
		//
		
		public string Path { get; }
		public bool IsLoading { get; }

		public virtual bool Load( Action onLoad = null )
		{
			throw new NotImplementedException();
		}

		public virtual bool Unload( Action onUnload = null )
		{
			throw new NotImplementedException();
		}
	}
}
