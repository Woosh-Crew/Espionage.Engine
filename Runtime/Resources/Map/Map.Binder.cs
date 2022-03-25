using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		public abstract class Binder
		{
			public virtual string Text { get; protected set; }
			public virtual float Progress { get; protected set; }

			public Scene Scene { get; protected set; }

			public abstract void Load( Action onLoad );

			public virtual void Unload()
			{
				Scene.Unload();
				Scene = default;
			}
		}

		[Group( "Maps" )]
		public abstract class File : IFile, IAsset, ILoadable
		{
			public Binder Binder { get; protected set; }

			public Library ClassInfo { get; }

			public File()
			{
				ClassInfo = Library.Register( this );
			}

			public FileInfo Info { get; set; }

			public abstract void Load( Action loaded );
			public abstract void Unload( Action finished );

			// ILoadable

			public virtual float Progress { get; protected set; }
			public virtual string Text => $"Loading File [{Info.Name}]";
		}
	}
}
