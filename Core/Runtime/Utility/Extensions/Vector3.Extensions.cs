using UnityEngine;

public static class Vector3Extensions
{
	public static Vector3 WithX( this Vector3 vector, float value )
	{
		return new Vector3( value, vector.y, vector.z );
	}

	public static Vector3 WithY( this Vector3 vector, float value )
	{
		return new Vector3( vector.x, value, vector.z );
	}

	public static Vector3 WithZ( this Vector3 vector, float value )
	{
		return new Vector3( vector.x, vector.y, value );
	}

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
