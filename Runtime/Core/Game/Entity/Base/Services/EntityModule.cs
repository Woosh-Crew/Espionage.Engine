using UnityEngine;

namespace Espionage.Engine
{
	internal class EntityModule : Module
	{
		protected override void OnUpdate()
		{
			// Run Think and Update
			foreach ( var entity in Entity.All )
			{
				(entity ? entity : null)?.Frame( UnityEngine.Time.deltaTime );
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
