using System;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		[Group( "Maps" )]
		public abstract class Binder : IBinder<Map>
		{
			public virtual float Progress { get; protected set; }
			public abstract string Identifier { get; }

			protected Scene Scene { get; set; }

			public abstract void Load( Action<Scene> onLoad = null );
			public abstract void Unload( Action onUnload = null );
		}
	}
}
