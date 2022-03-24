using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public sealed partial class Map
	{
		[Group( "Maps" )]
		public abstract class Binder : ILibrary
		{
			public Library ClassInfo { get; }

			public Binder()
			{
				ClassInfo = Library.Register( this );
			}

			public void Delete()
			{
				Library.Unregister( this );
				Scene = default;
			}

			// Binder

			public string Text { get; protected set; }
			public virtual float Progress { get; protected set; }
			public abstract string Identifier { get; }

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
			public Map Container { get; internal set; }

			public FileInfo Info { get; set; }
			public Library ClassInfo { get; }

			public File()
			{
				ClassInfo = Library.Register( this );
			}

			public abstract void Load( Action loaded );
			public abstract void Unload( Action finished );

			public void Delete()
			{
				Library.Unregister( this );
				Info = null;
			}

			// ILoadable

			public virtual float Progress { get; }
			public virtual string Text => $"Loading File [{Info.FullName}]";
		}
	}
}
