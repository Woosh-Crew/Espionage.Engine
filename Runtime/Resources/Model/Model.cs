using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Library( "res.model" ), File( Fallback = "models://w_error.umdl" ), Group( "Models" ), Path( "models", "assets://Models", Overridable = true )]
	public sealed class Model : IResource
	{
		public Library ClassInfo { get; }

		int IResource.Identifier { get; set; }
		public bool Persistant { get; set; }

		public Model()
		{
			ClassInfo = Library.Register( this );
		}

		public File Source { get; private set; }
		public GameObject Cache { get; set; }
		private List<Instance> Instances { get; set; } = new();

		public Instance Consume( Transform transform )
		{
			var instance = Instances[^1];

			Assert.IsTrue( instance.IsConsumed );
			instance.IsConsumed = true;
			instance.GameObject.SetActive( true );
			instance.GameObject.transform.parent = transform;
			instance.GameObject.transform.localPosition = Vector3.zero;

			return instance;
		}

		void IResource.Setup( string path )
		{
			var file = Files.Grab<File>( path );
			Assert.IsNull( file );

			Source = file;
		}

		void IResource.Load()
		{
			if ( Instances.Count <= 0 && Cache == null )
			{
				using var stopwatch = Debugging.Stopwatch( $"Loaded Model [{Files.Pathing( Source.Info ).Name()}]" );
				Source.Load( OnLoad );
			}

			Instances.Add( new( this ) );
		}

		private void OnLoad( GameObject gameObject )
		{
			Cache = gameObject;
		}

		bool IResource.Unload()
		{
			if ( !Persistant && Instances.Count <= 0 )
			{
				Debugging.Log.Info( $"Unloading Model [{Files.Pathing( Source.Info ).Name()}]" );

				Source.Unload();
				Cache = null;
			}

			return Instances.Count <= 0;
		}

		public sealed class Instance
		{
			public Instance( Model model )
			{
				Model = model;
				IsConsumed = false;

				GameObject = GameObject.Instantiate( Model.Cache );
				GameObject.SetActive( false );
			}

			public Model Model { get; private set; }
			public GameObject GameObject { get; private set; }
			public bool IsConsumed { get; set; }

			public void Delete()
			{
				Model.Instances.Remove( this );

				GameObject.Destroy( GameObject );
				Resource.Unload( Model );

				Model = null;
				GameObject = null;
			}
		}

		public static implicit operator Model( string value )
		{
			return Resource.Load<Model>( value );
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
