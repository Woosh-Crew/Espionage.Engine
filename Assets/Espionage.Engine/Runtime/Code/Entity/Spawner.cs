using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	[DisallowMultipleComponent]
	public class Spawner : MonoBehaviour
	{
		public Blueprint blueprint;

		void Start()
		{
			blueprint.Tree.Execute( "OnStart" );
		}
	}
}
