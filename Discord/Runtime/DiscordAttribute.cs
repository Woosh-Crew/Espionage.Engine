using System;

namespace Espionage.Engine.Steam
{
	[AttributeUsage( AttributeTargets.Class )]
	public class DiscordAttribute : Attribute, Library.IComponent
	{
		public uint Id { get; }

		public DiscordAttribute( uint appId )
		{
			AppId = appId;
		}

		public void OnAttached( ref Library library ) { }
	}
}
