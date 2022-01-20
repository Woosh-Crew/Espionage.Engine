using System;
using System.Collections;
using System.Collections.Generic;
using Discord;
using Espionage.Engine;

namespace Espionage.Engine.Discord
{
	internal static class DiscordInitializer
	{
		[Callback( "game.ready" )]
		private static void Initialize()
		{
			if ( !Engine.Game.ClassInfo.Components.TryGet<DiscordAttribute>( out var steam ) )
			{
				Debugging.Log.Warning( "No Discord component found on Game." );
				return;
			}
		}
	}
}
