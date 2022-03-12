using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	[Library, Title( "Build Index Map" ), Group( "Maps" )]
	public class BuildIndexMapProvider : Resource.IProvider<Map, Scene>
	{
		// Id
		public string Identifier => $"index:{_buildIndex}";

		// Outcome
		public Scene Output { get; private set; }

		// Loading Meta
		public float Progress => _operation.progress;
		public bool IsLoading { get; private set; }

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

		private AsyncOperation _operation;
		private readonly int _buildIndex;

		public void Load( Action finished )
		{
			IsLoading = true;

			_operation = SceneManager.LoadSceneAsync( _buildIndex );
			_operation.completed += ( _ ) =>
			{
				Output = SceneManager.GetSceneByBuildIndex( _buildIndex );
				finished?.Invoke();
			};
		}

		public void Unload( Action finished )
		{
			Output.Unload();
			Output = default;
		}
	}
}
