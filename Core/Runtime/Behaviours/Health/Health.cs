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

		[SerializeField]
		private int current;

		public void Damage() { }

		public void Heal() { }
	}
}
