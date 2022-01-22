using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Assets
{
	public sealed class Map : IResource
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public Map( string path )
		{
			Path = path;
		}

		//
		// IResource 
		//
		
		public string Path { get; }
		public bool IsLoading { get; private set; }
		public AssetBundle Bundle { get; private set; }

		public void Load( Action onLoad = null )
		{
			IsLoading = true;
			
			var request = AssetBundle.LoadFromFileAsync( Path );
			request.completed += (e) =>
			{
				// Finished Loading
				onLoad?.Invoke();
				IsLoading = false;
				Bundle = request.assetBundle;
			};
		}

		public void Unload(Action onUnload = null )
		{
			var request = Bundle.UnloadAsync(true);
			request.completed += ( e ) =>
			{
				onUnload?.Invoke();
			};
		}
	}
}
