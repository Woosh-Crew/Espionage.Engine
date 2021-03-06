using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources.Maps
{
	[Library, Title( "Build Index Map" ), Group( "Maps" )]
	public class BuildIndexMapProvider : Map.Binder
	{
		// Loading Meta
		public override float Progress
		{
			get
			{
				if ( _operation == null )
				{
					return 0;
				}

				return _operation.progress;
			}
		}

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

		public override void Load( Action finished )
		{
			var operation = SceneManager.LoadSceneAsync( _buildIndex, LoadSceneMode.Additive );
			operation.completed += ( _ ) =>
			{
				Scene = SceneManager.GetSceneByBuildIndex( _buildIndex );
				finished?.Invoke();
			};
		}

		public override void Unload( Action finished )
		{
			if ( Scene == default )
			{
				Scene = SceneManager.GetActiveScene();
			}

			base.Unload( finished );
		}
	}
}
