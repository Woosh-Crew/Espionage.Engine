using System.IO;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Library( "resources.umdl" ), Group( "Models" ), Title( "UMDL File" ), File( Extension = "umdl" )]
	public sealed class UMDL : IFile<Model, GameObject>, IAsset
	{
		public Library ClassInfo { get; } = Library.Database[typeof( UMDL )];

		// Resource

		public FileInfo File { get; set; }
		public void Load( FileStream fileStream ) { }

		// Provider

		public Resource.IProvider<Model, GameObject> Provider()
		{
			return new AssetBundleModelProvider( File );
		}
	}
}
