using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Allows an Entity to have Health, which means
	/// it can be healed or take damage.
	/// </summary>
	[Help( "Gives an Entity health, by implementing the IDamageable and IHealable interfaces." )]
	public class Health : Component, IDamageable, IHealable
	{
		[Property, Help( "The Current Health of this Entity" )]
		public int Current { get; private set; }

		[Property, Help( "The Max Health of this Entity" )]
		public int Max { get; set; } = 100;

		/// <summary> Damage this Entity, using some useful meta data. </summary>
		public void Damage( IDamageable.Info info )
		{
			if ( State == Mode.Dead && (!OnDamaged?.Invoke( ref info ) ?? false) )
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
			OnHealed?.Invoke( ref info );

			Current += info.Amount;
			Current = Mathf.Clamp( Current, 0, 100 );

			TimeSinceHealed = 0;
		}

		// Callbacks

		public event Damaged OnDamaged;
		public event Healed OnHealed;
		public event Killed OnKilled;

		public delegate void Healed( ref IHealable.Info info );

		public delegate bool Damaged( ref IDamageable.Info info );

		public delegate void Killed( IDamageable.Info info );

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
	}
}
