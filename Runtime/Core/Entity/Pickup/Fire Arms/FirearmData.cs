using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// The scriptable object for firearms.
	/// </summary>
	[CreateAssetMenu(fileName = "Firearm", menuName = "Pickups/Holdables/Weapons/Firearms/Firearm", order = 1)]
	public class FirearmData : WeaponData
	{	
		[Tab("Weapon")]
		/// <summary>The fire type of a given weapon</summary>
		public FireType Type;
		
		[Tab("Weapon")]
		//Public fields
		/// <summary>How far a weapon can shoot its bullets/stream, how far a projectile can travel before it is destroyed</summary>
		public float Range;

		[Tab("Weapon")]
		public float PrimaryFireRate;

		[Tab("Weapon")]
		public float SecondaryFireRate;
		
		[Tab("Weapon")]
		public float ReloadTime;

		[Tab("Weapon")]
		public GameObject Projectile;

		[Tab("Weapon")]
		[Range(0f,1f)]
		public float Spread;

		[Tab("Weapon")]
		/// <summary>Defines what layers can and cannot be hit by bullet raycasts</summary>
		public LayerMask BulletMask;
	}
}
