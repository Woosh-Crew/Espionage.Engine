using UnityEngine;

namespace Espionage.Engine.Entities.Nodes
{
	public sealed class Graph : ScriptableObject, ILibrary
	{
		public Library ClassInfo { get; set; }
	}
}
