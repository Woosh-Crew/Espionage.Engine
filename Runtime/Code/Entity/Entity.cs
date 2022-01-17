using UnityEngine;

namespace Espionage.Engine.Entities
{
	public abstract partial class Entity : Object, ILibrary, ICallbacks
	{
		public Blueprint Blueprint { get; }
		public Library ClassInfo { get; }

		public Entity()
		{
			Callback.Register( this );
			ClassInfo = Library.Database.Get( GetType() );
		}

		public Entity( Blueprint blueprint ) : this()
		{
			Blueprint = blueprint;
		}

		protected virtual void OnDestory()
		{
			Callback.Unregister( this );
		}
	}
}
