using UnityEngine;

namespace Espionage.Engine
{
	public class Health : Behaviour, IDamageable, IHealable
	{
		public int Current => current;
		
		[SerializeField]
		private int current;
		
		public void Damage()
		{
		}

		public void Heal()
		{
		}
	}
}
