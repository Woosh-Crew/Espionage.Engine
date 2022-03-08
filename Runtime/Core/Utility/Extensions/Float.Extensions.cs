using UnityEngine;

public static class FloatExtensions
{
	public static float Remap( this float input, float inputMin, float inputMax, float min, float max )
	{
		return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
	}

	public static float LerpTo( this float input, float b, float t )
	{
		return Mathf.Lerp( input, b, t );
	}
}
