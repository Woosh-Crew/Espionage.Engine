using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	public class Preloader
	{
#if UNITY_EDITOR

		[InitializeOnLoadMethod]
		private static void Setup()
		{

		}

#endif
	}
}
