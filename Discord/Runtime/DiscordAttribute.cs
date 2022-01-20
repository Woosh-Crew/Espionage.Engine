using System;

namespace Espionage.Engine.Discord
{
	[AttributeUsage( AttributeTargets.Class )]
	public class DiscordAttribute : Attribute, Library.IComponent
	{
		public uint Id { get; }

		public DiscordAttribute( uint id )
		{
			Id = id;
		}

		public void OnAttached( ref Library library ) { }
	}
}
