using System;
using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public class BreatheAnimation : Behaviour, Viewmodel.IEffect
	{
		private Vector3 _lastEnergyBreatheScale;
		private float _breatheBobDelta;

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			// Only do breathe if low energy
			_lastEnergyBreatheScale = _lastEnergyBreatheScale.LerpTo( energy.Amount.Remap( 95, 0, 0, 3.4f ), 25 * Time.deltaTime );

			var breatheSpeed = 0.6f * _lastEnergyBreatheScale.x;
			var breatheIntensity = 0.25f * _lastEnergyBreatheScale.z;

			_breatheBobDelta += (Time.deltaTime) * breatheSpeed;

			var breatheUp = MathF.Cos( _breatheBobDelta * 1f ) * 1.3f * breatheIntensity;
			var breatheLeft = MathF.Sin( _breatheBobDelta * 0.5f ) * 0.8f * breatheIntensity;
			var breatheForward = MathF.Cos( _breatheBobDelta * 0.5f ) * 0.5f * breatheIntensity;

			transform.position += Rotation.Left * breatheLeft + Rotation.Up * breatheUp;;
			transform.rotation *= Rotation.From( breatheUp * _lastEnergyBreatheScale.x, breatheLeft * _lastEnergyBreatheScale.y, breatheForward * _lastEnergyBreatheScale.z );
		}
	}
}
