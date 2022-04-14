using System.Linq;
using UnityEngine;

namespace Espionage.Engine
{
	public static class EntityExtensions
	{
		public static T MoveTo<T>( this T entity, Transform target ) where T : Entity
		{
			if ( target == null )
			{
				return entity;
			}

			entity.Position = target.position;
			entity.Rotation = target.rotation;
			return entity;
		}
		
		public static T Random<T>( this Entities database ) where T : Entity
		{
			return database.OfType<T>().Random();
		}
	}
}
