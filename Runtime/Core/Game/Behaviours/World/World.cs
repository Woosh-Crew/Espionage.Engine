using System;
using System.Linq;

namespace Espionage.Engine
{
	/// <summary>
	/// Central point of a map. Holds meta data about the map
	/// such as Custom NavMeshes, Environment Lighting, etc. 
	/// </summary>
	[Library( "env.world" ), Group( "Environment" ), Singleton, Icon( Path = "package://Assets/Icons/Entities/env_world.png" )]
	public class World : Entity
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
				_instance = All.OfType<World>().FirstOrDefault();
				return _instance != null ? _instance : null;
			}
		}

		protected override void OnAwake()
		{
			if ( _instance == null )
			{
				_instance = this;
				return;
			}

			Debugging.Log.Warning( "More than one world was present in scene" );
			Destroy( gameObject );
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
