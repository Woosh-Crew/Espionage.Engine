using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		[Group( "Maps" )]
		public abstract class Binder
		{
			public virtual float Progress { get; protected set; }
			public abstract string Identifier { get; }

			protected Scene Scene { get; set; }

			public abstract void Load( Action<Scene> onLoad = null );
			public abstract void Unload( Action onUnload = null );
		}

		[Group( "Maps" )]
		public abstract class File : IFile, IAsset
		{
			public abstract Binder Binder { get; }

			public Library ClassInfo { get; }

			public File()
			{
				ClassInfo = Library.Register( this );
			}

			~File()
			{
				Library.Unregister( this );
			}

			public FileInfo Source { get; set; }
			public void Load( FileStream fileStream ) { }
		}
	}
}
