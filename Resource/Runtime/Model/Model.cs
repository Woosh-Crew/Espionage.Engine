using System;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Group( "Models" )]
	public sealed class Model : Resource
	{
		private IModelProvider Provider { get; }
		public Components<Model> Components { get; }

		public override string Identifier => Provider.Identifier;

		private Model( IModelProvider provider )
		{
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

		public override bool IsLoading => Provider.IsLoading;

		public void Load( Action onLoad = null )
		{
			Provider.Load( onLoad );
		}

		public void Unload( Action onUnload = null )
		{
			Provider.Unload( onUnload );
		}

		public void Dispose() { }
	}
}
