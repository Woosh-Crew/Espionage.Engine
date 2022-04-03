using System;
using System.IO;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Group( "Models" ), Path( "models", "assets://Models/" )]
	public sealed class Model : Resource<GameObject>
	{
		public static Model Load( string path, Action<GameObject> onLoad = null )
		{
			path = Files.Pathing.Absolute( path );

			// Use the Database Map if we have it
			if ( Database[path] != null )
			{
				var model = Database[path] as Model;
				model?.Load( onLoad );
				return model;
			}

			if ( Files.Pathing.Exists( path ) )
			{
				var model = new Model( Files.Grab<File>( path, false ) );
				model?.Load( onLoad );
				return model;
			}

			Dev.Log.Error( $"Model Path [{path}], couldn't be found." );
			return null;
		}

		private Model( File provider )
		{
			Assert.IsNull( provider );

			Source = provider;

			Identifier = provider.Info.FullName;
			Database.Add( this );
		}

		public File Source { get; private set; }
		public GameObject Object { get; private set; }

		public int Instances { get; private set; }

		protected override void Load( Action<GameObject> onLoad )
		{
			if ( Instances <= 0 )
			{
				// Add to Database
				Database.Add( this );
				Source.Load( onLoad );
			}

			Instances++;
		}

		protected override void Unload()
		{
			Instances--;

			if ( Instances <= 0 )
			{
				Source.Unload();
				Database.Remove( this );
			}
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
