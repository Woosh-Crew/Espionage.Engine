using UnityEngine;

namespace Espionage.Engine
{
	public static class QuaternionExtensions
	{
		// Vector Math

		public static Vector3 Up( this Quaternion quaternion )
		{
			return quaternion * Vector3.up;
		}

		public static Vector3 Down( this Quaternion quaternion )
		{
			return quaternion * Vector3.down;
		}

		public static Vector3 Right( this Quaternion quaternion )
		{
			return quaternion * Vector3.right;
		}

		public static Vector3 Left( this Quaternion quaternion )
		{
			return quaternion * Vector3.left;
		}

		public static Vector3 Forward( this Quaternion quaternion )
		{
			return quaternion * Vector3.forward;
		}

		public static Vector3 Backward( this Quaternion quaternion )
		{
			return quaternion * Vector3.back;
		}

		// Axis

		public static float Pitch( this Quaternion quaternion )
		{
			return quaternion.eulerAngles.x;
		}

		public static float Yaw( this Quaternion quaternion )
		{
			return quaternion.eulerAngles.y;
		}

		public static float Roll( this Quaternion quaternion )
		{
			return quaternion.eulerAngles.z;
		}
	}
}
