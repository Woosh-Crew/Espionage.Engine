using System;

namespace Espionage.Engine.Resources
{	
	[Title( "Model" ), Group( "Models" ), File( Extension = "mdl" )]
	public class Model: IResource, IDisposable, IAsset, ILibrary
	{
		public Library ClassInfo { get; }
		
		private Model()
		{
			ClassInfo = Library.Database[GetType()];
		}

		public static Model Load( string path )
		{
			return null;
		}

		//
		// Resource
		//
		
		public string Identifier { get; }
		public bool IsLoading { get; }
		
		public bool Load( Action onLoad = null )
		{
			throw new NotImplementedException();
		}

		public bool Unload( Action onUnload = null )
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
