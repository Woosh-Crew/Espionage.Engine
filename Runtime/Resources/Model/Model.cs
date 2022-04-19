using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Library( "res.model" ), Group( "Models" ), Path( "models", "assets://<library_group(res.model)>/", Overridable = true )]
	public sealed class Model : ILibrary, IResource
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
		private Stack<Instance> Instances { get; set; } = new();

		public Instance Consume( Transform transform )
		{
			var instance = Instances.Peek();

			if ( instance.IsConsumed )
			{
				Assert.IsTrue( instance.IsConsumed );
				return null;
			}

			instance.IsConsumed = true;
			instance.GameObject.SetActive( true );
			instance.GameObject.transform.parent = transform;

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
				using var stopwatch = Debugging.Stopwatch( $"Loaded Model [{Files.Pathing.Name( Source.Info )}]" );
				Source.Load( OnLoad );
			}

			Instances.Push( new( this ) );
		}

		private void OnLoad( GameObject gameObject )
		{
			Cache = gameObject;
		}

		bool IResource.Unload()
		{
			Instances.Pop();

			if ( !Persistant && Instances.Count <= 0 )
			{
				Debugging.Log.Info( $"Unloading Model [{Files.Pathing.Name( Source.Info )}]" );

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

			public Model Model { get; }
			public GameObject GameObject { get; }
			public bool IsConsumed { get; set; }

			public void Delete()
			{
				GameObject.Destroy( GameObject );
				Resource.Unload( Model );
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
