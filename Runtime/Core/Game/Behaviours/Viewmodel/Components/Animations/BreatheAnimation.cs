using System;
using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class BreatheAnimation : Viewmodel.Effect
	{
		private Vector3 _lastEnergyBreatheScale;
		private float _breatheBobDelta;

		public override void PostCameraSetup( ref ITripod.Setup setup )
		{
			_breatheBobDelta += Time.deltaTime * breatheSpeed;

			var breatheUp = MathF.Cos( _breatheBobDelta * 1f ) * 1.3f * breatheIntensity;
			var breatheLeft = MathF.Sin( _breatheBobDelta * 0.5f ) * 0.8f * breatheIntensity;
			var breatheForward = MathF.Cos( _breatheBobDelta * 0.5f ) * 0.5f * breatheIntensity;

			var trans = transform;
			trans.position += trans.rotation * Vector3.left * breatheLeft + trans.rotation * Vector3.up * breatheUp;
			transform.rotation *= Quaternion.Euler( breatheUp * _lastEnergyBreatheScale.x, breatheLeft * _lastEnergyBreatheScale.y, breatheForward * _lastEnergyBreatheScale.z );
		}

		// Fields

		[SerializeField]
		private float breatheSpeed = 0.6f;

		[SerializeField]
		private float breatheIntensity = 0.25f;
	}
}
