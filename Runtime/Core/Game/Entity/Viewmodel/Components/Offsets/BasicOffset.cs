using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Tripods;
using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class BasicOffset : Viewmodel.Modifier
	{
		public override void PostCameraSetup( ref Tripod.Setup camSetup )
		{
			Position += Rotation * Vector3.left * offset.x + Rotation * Vector3.up * offset.y + Rotation * Vector3.forward * offset.z;
		}

		// Fields

		[SerializeField]
		private Vector3 offset;
	}
}
