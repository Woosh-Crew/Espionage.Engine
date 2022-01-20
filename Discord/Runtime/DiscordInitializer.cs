using System;
using System.Collections;
using System.Collections.Generic;
using Espionage.Engine;
using Steamworks;
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
				Debugging.Log.Warning( "No Steam component found on Game." );
				return;
			}

			try
			{
				SteamClient.Init( steam.AppId );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}

			if ( SteamClient.IsValid )
			{
				Callback.Run( "discord.ready" );
			}
		}
	}
}
