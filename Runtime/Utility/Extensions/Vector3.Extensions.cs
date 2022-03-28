using UnityEngine;

namespace Espionage.Engine
{
	public static class Vector3Extensions
	{
		// With

		public static Vector3 WithX( this Vector3 vector, float value )
		{
			return new( value, vector.y, vector.z );
		}

		public static Vector3 WithY( this Vector3 vector, float value )
		{
			return new( vector.x, value, vector.z );
		}

		public static Vector3 WithZ( this Vector3 vector, float value )
		{
			return new( vector.x, vector.y, value );
		}

		// Flip

		public static Vector3 FlipX( this Vector3 v )
		{
			return new( -v.x, v.y, v.z );
		}

		public static Vector3 FlipY( this Vector3 v )
		{
			return new( v.x, -v.y, v.z );
		}

		public static Vector3 FlipZ( this Vector3 v )
		{
			return new( v.x, v.y, -v.z );
		}

		// Lerp

		public static Vector3 LerpTo( this Vector3 input, Vector3 b, float t )
		{
			return Vector3.Lerp( input, b, t );
		}

		public static Vector3 SlerpTo( this Vector3 input, Vector3 b, float t )
		{
			return Vector3.Slerp( input, b, t );
		}

		public static float Dot( this Vector3 input, Vector3 b )
		{
			return Vector3.Dot( input, b );
		}
	}
}
