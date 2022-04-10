using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public class BodyGroups : Behaviour
	{
		[field: SerializeField]
		public Group[] Groups { get; set; }
		
		[Serializable]
		public struct Group
		{
			[field: SerializeField]
			public string Name { get; set; }
			
			[field: SerializeField]
			public Choice[] Objects { get; set; }
		}

		[Serializable]
		public struct Choice
		{
			[field: SerializeField]
			public string Name { get; set; }
			
			[field: SerializeField]
			public GameObject[] Objects { get; set; }
		}
	}
}
