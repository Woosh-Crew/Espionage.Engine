using System;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Group( "Models" ), Path( "models", "game://Models/" )]
	public sealed class Model : Resource
	{
		private IProvider<Model, GameObject> Provider { get; }
		public Components<Model> Components { get; }

		public override string Identifier => Provider.Identifier;

		private Model( IProvider<Model, GameObject> provider )
		{
			Components = new Components<Model>( this );
			Provider = provider;
		}

		// Resource

		public override bool IsLoading => Provider.IsLoading;

		protected override void OnLoad( Action onLoad )
		{
			Provider.Load( onLoad );
		}

		protected override void OnUnload( Action onUnload )
		{
			Provider.Unload( onUnload );
		}
	}
}
