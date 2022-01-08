using UnityEngine;

namespace Espionage.Engine.Entities
{
	public abstract class Entity : ILibrary, ICallbacks
	{
		public static IDatabase<Reference> Database { get; }

		public Library ClassInfo => Library.Database.Get( GetType() );

		public Entity()
		{
			Callback.Register( this );
		}

		~Entity()
		{
			Callback.Unregister( this );
		}
	}
}
