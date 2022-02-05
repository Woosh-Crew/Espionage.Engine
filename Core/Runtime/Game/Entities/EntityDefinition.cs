using UnityEngine;

namespace Espionage.Engine.Entities
{
	[CreateAssetMenu]
    public class EntityDefinition : ScriptableObject
    {
	    public Library library;
	    public GameObject prefab;

	    public GameObject Spawn()
	    {
		    return Instantiate( prefab );
	    }
    }
}
