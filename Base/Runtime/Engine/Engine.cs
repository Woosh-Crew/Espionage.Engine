using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	[Manager( nameof( Initialize ), Layer = Layer.Runtime, Order = 500 )]
	public class Engine
	{
		private static void Initialize() { }
	}
}
