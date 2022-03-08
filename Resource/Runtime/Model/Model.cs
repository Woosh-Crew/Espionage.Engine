using System;
using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Resources
{
	[Group( "Models" ), Path( "models", "game://Models/" )]
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
			if ( Database[path] is Model databaseModel )
			{
				((IResource)databaseModel).Load();
				return databaseModel;
			}

			var model = new Model( Files.Load<IFile<Model, GameObject>>( path ).Provider() );
			((IResource)model).Load( onLoad );
			return model;
		}

		// Spawning

		private List<GameObject> _spawned;

		public GameObject Spawn( Transform container )
		{
			var go = Object.Instantiate( Provider.Output, Vector3.zero, Quaternion.identity, container );
			_spawned.Add( go );
			return go;
		}

		public void Despawn( GameObject gameObject )
		{
			((IResource)this).Unload();
			_spawned.Remove( gameObject );
			Object.Destroy( gameObject );
		}

		// Resource

		public override bool IsLoading => Provider.IsLoading;

		protected override void OnLoad( Action onLoad )
		{
			Provider.Load( onLoad );
		}

		protected override void OnUnload( Action onUnload )
		{
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
