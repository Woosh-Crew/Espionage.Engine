using UnityEngine;

namespace Espionage.Engine
{
	public static class FloatExtensions
	{
		public static float LerpTo( this float input, float b, float t )
		{
			return Mathf.Lerp( input, b, t );
		}
	}
}
