using System;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Group( "Textures" ), Path( "textures", "game://Textures/" )]
	public sealed class Texture : Resource
	{
		private IProvider<Texture, Texture2D> Provider { get; }
		public Components<Texture> Components { get; }

		public override string Identifier => Provider.Identifier;

		private Texture( IProvider<Texture, Texture2D> provider )
		{
			Components = new( this );
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
