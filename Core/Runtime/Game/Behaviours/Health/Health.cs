using System;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Allows an Entity to have Health, which means
	/// it can be healed or take damage.
	/// </summary>
	public class Health : Component, IDamageable, IHealable
	{
		public int Current { get; private set; }
		public int Max => max;

		public void Damage( IDamageable.Info info ) { }

		public void Heal( IHealable.Info info )
		{
			Current += info.Amount;
			Current = Mathf.Clamp( Current, 0, 100 );
		}

		public Action OnKilled { get; set; }

		// Fields

		[SerializeField]
		private int max;
	}
}
