using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Blueprints
{
	[CreateAssetMenu]
    public class Blueprint : ScriptableObject
    {
	    [SerializeField]
	    private GameObject gameObject;
    }
}
