using UnityEngine;

namespace Espionage.Engine
{
	public interface IInputProcessor
	{
		Quaternion Rotation { get; }
		
		float Forward { get; }
		float Horizontal { get; }
	}
}
