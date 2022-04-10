using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public class MaterialGroups : Behaviour
	{
		[field: SerializeField]
		public Group[] Groups { get; set; }
		
		[Serializable]
		public struct Group
		{
			[field: SerializeField]
			public string Name { get; set; }
			
			[field: SerializeField]
			public Material Original { get; set; }
			
			[field: SerializeField]
			public Material New { get; set; }
		}
	}
}
