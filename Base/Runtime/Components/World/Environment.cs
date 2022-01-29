using System;
using UnityEngine;

namespace Espionage.Engine.World
{
	/// <summary>
	/// Environment generates world lighting
	/// </summary>
	[Group( "Maps.Lighting" )]
	public class Environment : ScriptableObject
	{
		[SerializeField]
		private Material skybox;
	}
}
