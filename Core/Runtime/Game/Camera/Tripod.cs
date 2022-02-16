using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// A Tripod is a camera controller behaviour. It controls the Main Camera in the game.
	/// </summary>
	public abstract class Tripod : Behaviour, ICamera
	{
		public virtual void Build( ref ICamera.Setup camSetup ) { }
		public virtual void Activated( ref ICamera.Setup camSetup ) { }
		public virtual void Deactivated() { }
	}
}
