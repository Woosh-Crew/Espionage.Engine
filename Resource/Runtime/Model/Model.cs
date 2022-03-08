using System;
using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Resources
{
	[Group( "Models" ), Path( "models", "assets://Models/" )]
	public sealed class Model : Resource
	{
		private IProvider<Model, GameObject> Provider { get; }
		public Components<Model> Components { get; }

		public override string Identifier => Provider.Identifier;

		private Model( IProvider<Model, GameObject> provider )
		{
			Components = new( this );
			Provider = provider;
		}

		public static Model Load( string path, Action onLoad = null )
		{
			path = Files.GetPath( path );

			if ( !Files.Exists( path ) )
			{
				return Load( "models://error.umdl", onLoad );
			}

			if ( Database[path] is Model databaseModel )
			{
				((IResource)databaseModel).Load();
				return databaseModel;
			}

			using var _ = Debugging.Stopwatch( $"Loading Model [{Files.GetPath( path )}]" );

			var model = new Model( Files.Load<IFile<Model, GameObject>>( path ).Provider() );
			((IResource)model).Load( onLoad );
			return model;
		}

		// Spawning

		private List<GameObject> _spawned = new();

		public GameObject Spawn( Transform container )
		{
			var go = Object.Instantiate( Provider.Output, container );
			go.transform.localPosition = Vector3.zero;
			_spawned.Add( go );
			return go;
		}

		public void Destroy( GameObject gameObject )
		{
			_spawned.Remove( gameObject );
			((IResource)this).Unload();

			if ( Application.isEditor )
			{
				Object.DestroyImmediate( gameObject );
			}
			else
			{
				Object.Destroy( gameObject );
			}
		}

		// Resource

		public override bool IsLoading => Provider.IsLoading;

		protected override void OnLoad( Action onLoad )
		{
			Provider.Load( onLoad );
		}

		protected override void OnUnload( Action onUnload )
		{
			using var _ = Debugging.Stopwatch( $"Unloading Model [{Files.GetPath( Identifier )}]" );

			// Clear Spawned Models
			foreach ( var instance in _spawned )
			{
				Object.Destroy( instance );
			}

			_spawned.Clear();
			_spawned = null;

			// Tell Provider to Unload
			Provider.Unload( onUnload );
		}
	}
}
