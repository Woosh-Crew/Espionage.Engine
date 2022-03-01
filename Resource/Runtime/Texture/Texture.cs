using System;
using Espionage.Engine.Components;

namespace Espionage.Engine.Resources
{
	[Group( "Textures" ), Path( "textures", "game://Textures/" )]
	public sealed class Texture : Resource
	{
		private ITextureProvider Provider { get; }
		public Components<Texture> Components { get; }

		public override string Identifier => Provider.Identifier;

		private Texture( ITextureProvider provider )
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
