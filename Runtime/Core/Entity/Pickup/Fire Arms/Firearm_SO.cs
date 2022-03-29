using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// The scriptable object for firearms.
	/// </summary>
	[CreateAssetMenu(fileName = "Firearm", menuName = "Pickups/Holdables/Weapons/Firearms/Firearm", order = 1)]
	public class Firearm_SO : Weapon_SO
	{
		/// <summary>The fire type of a given weapon</summary>
		public FireType Type;

		//Public fields
		/// <summary>How far a weapon can shoot its bullets/stream, how far a projectile can travel before it is destroyed</summary>
		public float Range{ get; set; }
	}
}
