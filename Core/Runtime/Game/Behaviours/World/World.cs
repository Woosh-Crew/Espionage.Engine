using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Central point of a map.
	/// </summary>
	[Library( "env.world" ), Group( "Environment" )]
	public class World : Behaviour
	{
		// Singleton
		private static World _instance;

		public static World Instance
		{
			get
			{
				// If we already have instance, return it
				if ( _instance != null )
				{
					return _instance;
				}

				// Find the instance
				_instance = FindObjectOfType<World>();
				return _instance != null ? _instance : null;
			}
		}

		protected override void OnAwake()
		{
			if ( _instance == null )
			{
				_instance = this;
			}
			else
			{
				Debugging.Log.Warning( "More than one world was present in scene" );
				Destroy( gameObject );
			}
		}

		protected override void OnDelete()
		{
			if ( _instance == this )
			{
				_instance = null;
			}
		}
	}
}
