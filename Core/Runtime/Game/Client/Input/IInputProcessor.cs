using UnityEngine;

namespace Espionage.Engine
{
	public interface IInputProcessor
	{
		Vector2 ViewAngles { get; }

		float Forward { get; }
		float Horizontal { get; }
	}
}
