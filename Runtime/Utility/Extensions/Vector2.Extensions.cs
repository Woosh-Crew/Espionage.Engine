using UnityEngine;

namespace Espionage.Engine
{
	public static class Vector2Extensions
	{
		public static Vector2 WithX( this Vector2 vector, float value )
		{
			return new Vector2( value, vector.y );
		}

		public static Vector2 WithY( this Vector2 vector, float value )
		{
			return new Vector2( vector.x, value);
		}

		public static Vector2 LerpTo( this Vector2 input, Vector2 b, float t )
		{
			return Vector2.Lerp( input, b, t );
		}

		public static float Dot( this Vector2 input, Vector2 b )
		{
			return Vector2.Dot( input, b );
		}
	}
}
