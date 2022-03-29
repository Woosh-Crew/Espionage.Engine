using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
    /// <summary>
	/// The scriptable object for firearms.
	/// </summary>
	[CreateAssetMenu(fileName = "Weapon", menuName = "Pickups/Holdables/Weapons/Weapon", order = 1)]
	public class Weapon_SO : Holdable_SO
	{	

		[Tab("Weapon")]
		/// <summary>The given damage a weapon does</summary>
		public float Damage;

		public List<WeaponSound> Sounds;

	}

	[System.Serializable]
	public class WeaponSound{
		public string Name;
		public AudioClip[] Sounds;
		public float PitchVariance; 
	}
}
