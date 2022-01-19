using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Steamworks;
using UnityEngine;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Runtime | Layer.Editor, Order = 500 )]
	public static class Engine
	{
		public static Game Game { get; private set; }

		private static void Initialize()
		{
			var target = Library.Database.GetAll<Game>().FirstOrDefault( e => !e.Class.IsAbstract );
			if ( target is null )
			{
				Debugging.Log.Warning( "Game couldn't be found." );
				return;
			}

			Game = Library.Database.Create<Game>( target.Class );

			// Init Steam/
			try
			{
				SteamClient.Init( Game.AppId );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}
		}
	}
}
