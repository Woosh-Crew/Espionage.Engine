using System;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Allows a GameObject to have Health, which means
	/// it can be healed or take damage.
	/// </summary>
	public class Health : Behaviour, IDamageable, IHealable
	{
		public int Current { get; private set; }
		public int Max => max;

		public void Damage() { }
		public void Heal() { }

		public Action OnKilled { get; set; }

		// Fields

		[SerializeField]
		private int max;
	}
}
