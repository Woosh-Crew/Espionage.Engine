using System;
using UnityEngine;

namespace Espionage.Engine.Pickups.Viewmodels
{
	public class BobAnimation : Behaviour, Viewmodel.IEffect
	{
		private Vector3 _velocity;
		private float _walkBobDelta;
		private float _walkBobScale;
		private float _dampedSpeed;
		private Vector3 _lastWalkBob;

		public void PostCameraSetup( ref ITripod.Setup camSetup )
		{
			_velocity -= transform.position;

			var speed = Mathf.InverseLerp( 0, 240, _velocity.magnitude );

			_dampedSpeed = Mathf.Lerp( _dampedSpeed, speed, 2 * Time.deltaTime );
			_walkBobScale = Mathf.Lerp( _walkBobScale, speed, 10 * Time.deltaTime );
			_walkBobDelta += Time.deltaTime * 15.0f * _walkBobScale;

			// Waves
			_lastWalkBob.x = MathF.Sin( _walkBobDelta * 0.7f ) * 0.6f;
			_lastWalkBob.y = MathF.Cos( _walkBobDelta * 0.7f ) * 0.4f;
			_lastWalkBob.z = MathF.Cos( _walkBobDelta * 1.3f ) * 0.8f;

			// Scale walk bob off property
			_lastWalkBob *= _dampedSpeed;

			var trans = transform;
			trans.position += trans.rotation * Vector3.up * _lastWalkBob.z + trans.rotation * Vector3.left * _lastWalkBob.y * 1.25f;
			transform.rotation *= Quaternion.Euler( _lastWalkBob.z * 2, _lastWalkBob.y * 4, _lastWalkBob.x * 4 );
		}
	}
}
