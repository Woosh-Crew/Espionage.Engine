using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Struct containing data about the mouse
	/// </summary>
	public struct Mouse
	{
		public Vector2 Delta { get; internal set; }
		public float Wheel { get; internal set; }
	}
}
