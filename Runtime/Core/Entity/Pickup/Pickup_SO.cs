using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Espionage.Engine
{
	[CreateAssetMenu(fileName = "Pickup", menuName = "Pickups/Pickup", order = 1)]
    public class Pickup_SO : ScriptableObject
    {		
			[Tab("Tab 1")]
			public int TestInt;

			[Tab("Tab 1")]
			public int TestInt2;

			[Tab("Tab 2")]
			public List<int> ListOfInts;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class TabAttribute : Attribute{
	public readonly string Name;
	public TabAttribute(string n){
		Name = n;
	}
}

