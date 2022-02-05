using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	public class EntityDatabase : ScriptableObject
	{
		public List<EntityDefinition> definitions;

		public void Cache()
		{
			foreach ( var item in definitions )
			{
				Library.Database.Add( item.library );
			}
		}
	}
}
