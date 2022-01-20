using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Steamworks;
using UnityEditor;
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

			// Setup Callbacks
			Application.quitting -= OnShutdown;
			Application.quitting += OnShutdown;

			// Ready Up Project
			Callback.Run( "game.ready" );
			Game.OnReady();

			// Setup PlayerSettings based off of Project
#if UNITY_EDITOR
			PlayerSettings.productName = Game.ClassInfo.Title;

			if ( Game.ClassInfo.Components.TryGet<CompanyAttribute>( out var item ) )
			{
				PlayerSettings.companyName = item.Company;
			}
#endif
		}

		private static void OnShutdown()
		{
			Game.OnShutdown();
		}
	}
}
