using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Espionage.Engine
{
	[CreateAssetMenu(fileName = "Pickup", menuName = "Pickups/Pickup", order = 1)]
    public class Pickup_SO : ScriptableObject
    {		
		
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class TabAttribute : Attribute{
	public readonly string Name;
	public TabAttribute(string n){
		Name = n;
	}
}

