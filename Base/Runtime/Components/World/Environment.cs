using System;
using UnityEngine;

namespace Espionage.Engine.World
{
	/// <summary>
	/// Environment generates world lighting. This includes
	/// the Directional light, skybox, fog, Post Processing, etc.
	/// </summary>
	[Group( "Maps.Lighting" )]
	public class Environment : ScriptableObject
	{
		[SerializeField]
		private Material skybox;
	}
}
