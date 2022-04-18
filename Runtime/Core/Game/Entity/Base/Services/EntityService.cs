using UnityEngine;

namespace Espionage.Engine.Services
{
	internal class EntityService : Service
	{
		public override void OnUpdate()
		{
			foreach ( var entity in Entity.All )
			{
				(entity ? entity : null)?.Thinking.Run();
			}
		}

		// Spawn Entities

		[Function, Callback( "map.loaded" )]
		public void OnMapLoaded()
		{
			foreach ( var proxy in GameObject.FindObjectsOfType<Proxy>() )
			{
				var ent = proxy.Create();
				if ( ent == null )
				{
					continue;
				}

				ent.Spawn();
			}
		}
	}
}
