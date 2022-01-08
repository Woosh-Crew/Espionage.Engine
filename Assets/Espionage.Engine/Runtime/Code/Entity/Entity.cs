using UnityEngine;

namespace Espionage.Engine.Entities
{
	public abstract class Entity : ILibrary, ICallbacks
	{
		public static IDatabase<Blueprint> Database { get; }

		public Library ClassInfo { get; set; }

		public Entity()
		{
			Callback.Register( this );

			ClassInfo = Library.Database.Get( GetType() );
		}

		~Entity()
		{
			Callback.Unregister( this );
		}
	}
}
