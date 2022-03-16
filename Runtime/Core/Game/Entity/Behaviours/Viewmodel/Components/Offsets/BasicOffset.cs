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
			var trans = transform;
			trans.position += trans.rotation * Vector3.left * offset.x + trans.rotation * Vector3.up * offset.y + trans.rotation * Vector3.forward * offset.z;
		}

		// Fields

		[SerializeField]
		private Vector3 offset;
	}
}
