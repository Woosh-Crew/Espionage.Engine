using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	public class Spawner : MonoBehaviour
	{
		public Blueprint blueprint;
		
		// Start is called before the first frame update
		private void Start()
		{
			var newBp = blueprint.Spawn();
			Debug.Log(newBp);
		}
	}

}
