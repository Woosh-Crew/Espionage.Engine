using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
    public class BuildIndexMapProvider : IMapProvider
    {
	    // Id
	    public string Identifier => $"index:{_buildIndex}";
	    
	    // Outcome
	    public Scene? Scene { get; private set; }
	    
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
			    Scene = SceneManager.GetSceneByBuildIndex( _buildIndex );
			    finished?.Invoke();
		    };
	    }

	    public void Unload( Action finished )
	    {
		    Scene?.Unload();
		    Scene = null;
	    }
    }
}
