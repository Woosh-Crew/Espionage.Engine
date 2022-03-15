using System;
using System.IO;
using UnityEngine;

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
				ClassInfo= Library.Register(this);
			}

			~Binder()
			{
				Library.Unregister(this);
			}
			
			public virtual float Progress { get; protected set; }
			public abstract string Identifier { get; }
			
			public abstract void Load( Action onLoad = null );
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
			public void Load( FileStream fileStream ) { }
		}
	}
}
