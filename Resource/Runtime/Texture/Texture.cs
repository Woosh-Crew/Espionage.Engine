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
			Components = new Components<Texture>( this );
			Provider = provider;
		}


		// Resource

		public override bool IsLoading => Provider.IsLoading;

		public override void Load( Action onLoad = null )
		{
			base.Load( onLoad );

			Provider.Load( onLoad );
		}

		public override void Unload( Action onUnload = null )
		{
			base.Unload( onUnload );

			Provider.Unload( onUnload );
		}
	}
}
