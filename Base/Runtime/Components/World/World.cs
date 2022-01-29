using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.World
{
	[Group( "Maps" )]
    public class World : Behaviour
    {
	    public Environment Environment => environment; 
	    
	    [SerializeField]
	    private Environment environment;
    }
}
