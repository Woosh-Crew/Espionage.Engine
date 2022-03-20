using UnityEngine;

namespace Espionage.Engine
{
	public static class QuaternionExtensions
	{
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
