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
	}
}
