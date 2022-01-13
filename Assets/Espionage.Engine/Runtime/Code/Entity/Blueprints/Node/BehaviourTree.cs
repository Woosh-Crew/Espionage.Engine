using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	[CreateAssetMenu( fileName = "Espionage.Engine/Tree" )]
	public class BehaviourTree : ScriptableObject
	{
		public List<Node> Roots { get; set; }
	}
}
