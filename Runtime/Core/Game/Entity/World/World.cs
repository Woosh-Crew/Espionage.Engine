namespace Espionage.Engine
{
	/// <summary>
	/// Central point of a map. Holds meta data about the map
	/// such as Custom NavMeshes, Environment Lighting, etc. 
	/// </summary>
	[Library( "env.world" ), Group( "Environment" ), Singleton]
	public class World : Entity
	{
		// Singleton
		// --------------------------------------------------------------------------------------- //

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
				_instance = All.Find<World>();
				return _instance;
			}
		}

		protected override void OnSpawn()
		{
			if ( _instance == null )
			{
				_instance = this;
				return;
			}

			Debugging.Log.Warning( "More than one world was present in scene" );
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
