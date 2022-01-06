using UnityEngine;

namespace Espionage.Engine.Internal
{
	internal class EntityLink : MonoBehaviour
	{
		internal Entity Entity { get; set; }

		public static void Create( Entity entity )
		{
			var link = new GameObject( $"Link / {entity.ClassInfo.Name}" ).AddComponent<EntityLink>();
			entity.SetLink( link );

			link.Entity = entity;
		}
	}
}
