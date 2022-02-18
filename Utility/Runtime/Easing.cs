using UnityEngine;

namespace Espionage.Engine
{
	public static class Easing
	{
		public static float Spring( float start, float end, float value )
		{
			value = Mathf.Clamp01( value );
			value = (Mathf.Sin( value * Mathf.PI * (0.2f + 2.5f * value * value * value) ) * Mathf.Pow( 1f - value, 2.2f ) + value) * (1f + 1.2f * (1f - value));
			return start + (end - start) * value;
		}

		private static float Flip( float x )
		{
			return 1 - x;
		}

		public static float EaseIn( float t )
		{
			return t * t;
		}

		public static float EaseOut( float t )
		{
			return Flip( Mathf.Sqrt( Flip( t ) ) );
		}

		public static float Spike( float t )
		{
			return t <= .5f ? EaseIn( t / .5f ) : EaseIn( Flip( t ) / .5f );
		}
	}
}
