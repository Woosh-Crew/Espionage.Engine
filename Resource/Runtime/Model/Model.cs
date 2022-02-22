using System;
using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	[Group( "Models" )]
	public sealed partial class Model : IResource, IDisposable, ILibrary
	{
		public Library ClassInfo { get; }
		public Components<Model> Components { get; }

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
			Components = new Components<Model>( this );

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

		public void Load( Action onLoad = null )
		{
			onLoad += () =>
			{
				Callback.Run( "map.loaded" );
			};

			Provider.Load( onLoad );
		}


		public void Unload( Action onUnload = null )
		{
			onUnload += () =>
			{
				Callback.Run( "map.loaded" );
			};

			Provider.Unload( onUnload );
		}

		public void Dispose() { }
	}
}
