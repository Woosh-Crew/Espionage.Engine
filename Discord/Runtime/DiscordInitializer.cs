using System;
using System.Collections;
using System.Collections.Generic;
using Espionage.Engine;
using UnityEngine;

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
