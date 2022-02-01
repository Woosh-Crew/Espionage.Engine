using System;
using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	[Title( "Model" ), Group( "Models" ), File( Extension = "mdl" )]
	public sealed partial class Model : IResource, IDisposable, IAsset, ILibrary
	{
		public Library ClassInfo { get; }
		public ComponentDatabase<Model> Components { get; }

		//
		// Meta Data
		//

		private IModelProvider Provider { get; }

		public string Identifier => Provider.Identifier;
		public string Title { get; set; }
		public string Description { get; set; }

		//
		// Constructors
		//

		private Model( IModelProvider provider )
		{
			ClassInfo = Library.Database[GetType()];
			Components = new ComponentDatabase<Model>( this );

			Provider = provider;
		}

		public static Model Load( string path )
		{
			return null;
		}

		//
		// Resource
		//

		public bool IsLoading => Provider.IsLoading;

		public bool Load( Action onLoad = null )
		{
			throw new NotImplementedException();
		}

		private void Internal_LoadRequest( Action onLoad = null )
		{
			onLoad += () =>
			{
				Callback.Run( "map.loaded" );
			};

			Provider.Load( onLoad );
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
