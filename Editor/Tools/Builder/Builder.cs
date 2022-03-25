using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Tools.Editor
{
	[CreateAssetMenu]
	public class Builder : ScriptableObject
	{
		[field: SerializeField]
		public List<Action> Actions { get; set; }

		public class Action : ScriptableObject
		{
		}
	}
}
