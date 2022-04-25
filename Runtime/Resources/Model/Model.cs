using System;
using System.IO;
using Espionage.Engine.IO;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Library( "mdl.asset" ), File( Fallback = "models://w_error.umdl" ), Group( "Models" ), Path( "models", "assets://Models", Overridable = true )]
	public sealed class Model : IAsset
	{
		public Library ClassInfo { get; }
		public Resource Resource { get; set; }

		public Model()
		{
			ClassInfo = Library.Register( this );
		}

		public File Bundle { get; private set; }
		public GameObject Cache { get; set; }

		void IAsset.Setup( Pathing path )
		{
			var file = Files.Grab<File>( path );
			Assert.IsNull( file );

			Bundle = file;
		}

		void IAsset.Load()
		{
			using var stopwatch = Debugging.Stopwatch( $"Loaded Model [{Files.Pathing( Bundle.Info ).Name()}]" );
			Bundle.Load( OnLoad );
		}

		private void OnLoad( GameObject gameObject )
		{
			Cache = gameObject;
		}

		void IAsset.Unload()
		{
			Debugging.Log.Info( $"Unloading Model [{Files.Pathing( Bundle.Info ).Name()}]" );

			Bundle.Unload();
			Cache = null;
		}

		public static implicit operator Model( string value )
		{
			return Assets.Load<Model>( value );
		}

		[Library( "mdl.file" ), Group( "Models" )]
		public abstract class File : IFile
		{
			public Library ClassInfo { get; }

			protected File()
			{
				ClassInfo = Library.Register( this );
			}

			public FileInfo Info { get; set; }

			public abstract void Load( Action<GameObject> loaded );
			public abstract void Unload();
		}
	}
}
