using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Resources
{
	public partial class Texture
	{
		[Group( "Textures" )]
		public abstract class Binder : ILibrary
		{
			public Library ClassInfo { get; }

			public Binder()
			{
				ClassInfo = Library.Register( this );
			}

			~Binder()
			{
				Library.Unregister( this );
			}

			public abstract string Identifier { get; }
			public abstract Texture2D Texture { get; set; }

			public abstract void Load( Action<Texture2D> onLoad = null );
			public abstract void Unload( Action onUnload = null );
		}

		[Group( "Textures" )]
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
			public virtual void Load( FileStream fileStream ) { }
		}
	}
}
