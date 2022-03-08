using UnityEngine;

namespace Espionage.Engine
{
	public interface IDamageable
	{
		void Damage( Info info );

		public struct Info
		{
			public static implicit operator Info( int value )
			{
				return new()
				{
					Amount = value
				};
			}

			public int Amount { get; set; }
			public float Force { get; set; }

			public Vector3 Impact { get; set; }
			public Vector3 Normal { get; set; }

			public Actor Attacker { get; set; }
		}
	}
}
