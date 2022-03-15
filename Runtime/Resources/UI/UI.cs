using System;

namespace Espionage.Engine.Resources
{
	[Group( "User Interfaces" ), Title( "User Interface" ), Path( "ui", "assets://Interfaces/" )]
	public partial class UI : Resource
	{
		public override string Identifier { get; }
		private Binder Provider { get; }

		private UI( Binder provider )
		{
			Provider = provider ?? throw new NullReferenceException();
			Database.Add( this );
		}

		public override void Load( Action loaded = null )
		{
			base.Load( loaded );
			Provider.Load( loaded );
		}

		public override void Unload( Action unloaded = null )
		{
			base.Unload( unloaded );
			Provider.Unload( unloaded );
		}
	}
}
