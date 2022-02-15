using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Allows a GameObject to have Health, which means
	/// it can be healed or take damage.
	/// </summary>
	public class Health : Behaviour, IDamageable, IHealable
	{
		public int Current => current;
		public int Max => max;

		[SerializeField]
		private int current;

		[SerializeField]
		private int max;

		public void Damage() { }

		public void Heal() { }
	}
}
