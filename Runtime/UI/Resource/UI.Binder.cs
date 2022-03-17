using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Resources
{
	public partial class UI
	{
		[Group( "User Interfaces" )]
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
			public abstract Canvas Canvas { get; set; }

			public abstract void Load( Action<Canvas> onLoad = null );
			public abstract void Unload( Action onUnload = null );
		}

		[Group( "User Interfaces" )]
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
