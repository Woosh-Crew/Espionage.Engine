using DiscordAPI;
using UnityEngine;

namespace Espionage.Engine.Discord
{
	internal static class DiscordInitializer
	{
		[Callback( "game.ready" )]
		private static void Initialize()
		{
			if ( !Engine.Game.ClassInfo.Components.TryGet<DiscordAttribute>( out var discordId ) )
			{
				Debugging.Log.Warning( "No Discord component found on Game." );
				return;
			}

			Discord.Current = new DiscordAPI.Discord( discordId.Id, (ulong)CreateFlags.NoRequireDiscord );
			Application.onBeforeRender += RunCallback;
		}

		private static void RunCallback()
		{
			if ( Discord.Current is null )
			{
				return;
			}

			try
			{
				Discord.Current.RunCallbacks();
			}
			catch ( ResultException e )
			{
				if ( e.Result is Result.NotRunning )
				{
					Discord.Current = null;
					return;
				}

				Debugging.Log.Error( $"Error running Discord callbacks: {e}." );
			}
		}
	}
}
