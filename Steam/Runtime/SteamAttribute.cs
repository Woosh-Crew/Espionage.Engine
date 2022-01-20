using System;

namespace Espionage.Engine.Steam
{
	[AttributeUsage( AttributeTargets.Class )]
	public class SteamAttribute : Attribute, Library.IComponent
	{
		public uint AppId { get; }

		public SteamAttribute( uint appId )
		{
			AppId = appId;
		}

		public void OnAttached( ref Library library ) { }
	}
}
