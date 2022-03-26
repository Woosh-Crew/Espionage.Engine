using UnityEngine;

namespace Espionage.Engine
{
	public static class FloatExtensions
	{
		public static float Remap( this float input, float inputMin, float inputMax, float min, float max )
		{
			return EMath.Remap( input, inputMin, inputMax, min, max );
		}

		public static float LerpTo( this float input, float b, float t )
		{
			return Mathf.Lerp( input, b, t );
		}
	}
}
