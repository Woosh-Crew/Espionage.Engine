using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Build Index Map" ), Group( "Maps" )]
	public class BuildIndexMapProvider : Resource.IProvider<Map>
	{
		// Id
		public string Identifier => $"index:{_buildIndex}";

		// Loading Meta
		public float Progress => _operation.progress;

		public BuildIndexMapProvider( int index )
		{
			if ( index > SceneManager.sceneCountInBuildSettings )
			{
				throw new InvalidOperationException( "No matching index in build settings" );
			}

			_buildIndex = index;
		}

		//
		// Resource
		//

		private Scene _scene;

		private AsyncOperation _operation;
		private readonly int _buildIndex;

		public void Load( Action finished )
		{
			SceneManager.LoadScene( _buildIndex );

			_scene = SceneManager.GetSceneAt( _buildIndex );
			SceneManager.SetActiveScene( _scene );

			finished?.Invoke();
		}

		public void Unload( Action finished )
		{
			if ( _scene == default )
			{
				_scene = SceneManager.GetActiveScene();
			}

			var request = _scene.Unload();
			request.completed += _ => finished?.Invoke();
		}
	}
}
