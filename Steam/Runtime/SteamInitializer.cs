using System;
using System.Collections;
using System.Collections.Generic;
using Espionage.Engine;
using Steamworks;
using UnityEngine;

namespace Espionage.Engine.Steam
{
	internal static class SteamInitializer
	{
		[Callback( "game.ready" )]
		private static void Initialize()
		{
			if ( !Engine.Game.ClassInfo.Components.TryGet<SteamAttribute>( out var steam ) )
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
				Callback.Run( "steam.ready" );
			}
		}
	}
}
