using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public class Sockets : Behaviour
	{
		[field: SerializeField]
		public Group[] Groups { get; set; }
		
		[Serializable]
		public struct Group
		{
			[field: SerializeField]
			public string Name { get; set; }
			
			[field: SerializeField]
			public Transform Origin { get; set; }
		}
	}
}
