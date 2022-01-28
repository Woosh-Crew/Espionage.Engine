using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
    public abstract class Entity : Object, ILibrary, ICallbacks
    {
	    public static List<Entity> All { get; }
	    
	    static Entity()
	    {
		    All = new List<Entity>();
	    }

	    // Entity

	    public Library ClassInfo { get; set; }

	    public Entity()
	    {
		    ClassInfo = Library.Database[GetType()];
		    Callback.Register( this );
		    All.Add( this );
		    
		    CreateHook();
	    }

	    private void OnDestroy()
	    {
		    All.Remove( this );
		    Callback.Unregister( this );
		    
		    DeleteHook();
	    }
	    
	    // Hook

	    public GameObject Hook => _gameObject;
	    private GameObject _gameObject;

	    private void CreateHook()
	    {
		    _gameObject = new GameObject( ClassInfo.Name );
		    
		    // Add Entity reference component
	    }

	    private void DeleteHook()
	    {
		    Destroy(_gameObject);
	    }
	    
	    // Validation

	    public virtual bool IsValid()
	    {
		    return _gameObject != null;
	    }
	    
	    //
	    // Components
	    //
    }
}
