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

		/// <summary> Damage this Entity, using some useful meta data. </summary>
		public void Damage( IDamageable.Info info )
		{
			if ( State == Mode.Dead && (!OnDamaged?.Invoke( info ) ?? false) )
			{
				return;
			}

			Current -= info.Amount;
			Current = Mathf.Clamp( Current, 0, 100 );
			TimeSinceDamaged = 0;

			if ( Current == 0 )
			{
				OnKilled?.Invoke( info );
				TimeSinceKilled = 0;
			}
		}

		/// <summary> Heal this Entity, using some useful meta data. </summary>
		public void Heal( IHealable.Info info )
		{
			Current += info.Amount;
			Current = Mathf.Clamp( Current, 0, 100 );

			OnHealed?.Invoke( info );
			TimeSinceHealed = 0;
		}

		// Callbacks

		public event Func<IDamageable.Info, bool> OnDamaged;

		public event Action<IHealable.Info> OnHealed;
		public event Action<IDamageable.Info> OnKilled;

		// Helpers

		/// <summary> How long has it been since I've been last healed </summary>
		public TimeSince TimeSinceHealed { get; private set; }

		/// <summary> How long has it been since I've been killed </summary>
		public TimeSince TimeSinceKilled { get; private set; }

		/// <summary> How long has it been since I've been damaged </summary>
		public TimeSince TimeSinceDamaged { get; private set; }

		/// <summary> Am I dead or alive? https://youtu.be/SRvCvsRp5ho </summary>
		public Mode State => Current == 0 ? Mode.Dead : Mode.Alive;

		public enum Mode
		{
			Alive,
			Dead
		}

		// Fields

		[SerializeField]
		private int max;
	}
}
