using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
    public abstract class Asset : ScriptableObject, ILibrary
    {
	    public Library ClassInfo { get; private set; }

	    private void OnEnable()
	    {
		    ClassInfo = Library.Database[GetType()];
	    }
    }
}
