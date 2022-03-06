using System;
using UnityEngine;

namespace Espionage.Engine.Pickups.Viewmodels
{
	public class BobAnimation : Component, Viewmodel.IEffect
	{
		private Vector3 Velocity { get; set; }
		private Vector3 _lastPosition;

		private float _walkBobDelta;
		private float _walkBobScale;
		private float _dampedSpeed;
		private Vector3 _lastWalkBob;

		public void PostCameraSetup( ref ITripod.Setup camSetup )
		{
			//Set our velocity to the vector between our last position and this position
			var position = Local.Pawn.transform.position;

			Velocity = (position - _lastPosition) / Time.deltaTime;
			_lastPosition = position;

			var speed = Velocity.magnitude;

			_dampedSpeed = Mathf.Lerp( _dampedSpeed, speed, 2 * Time.deltaTime );
			_walkBobScale = Mathf.Lerp( _walkBobScale, speed, 10 * Time.deltaTime );
			_walkBobDelta += Time.deltaTime * bobSpeedScale * _walkBobScale;

			// Waves
			_lastWalkBob.x = MathF.Sin( _walkBobDelta * 0.7f ) * 0.6f;
			_lastWalkBob.y = MathF.Cos( _walkBobDelta * 0.7f ) * 0.4f;
			_lastWalkBob.z = MathF.Cos( _walkBobDelta * 1.3f ) * 0.8f;

			// Scale walk bob off property
			_lastWalkBob *= _dampedSpeed * bobBounceScale;

			var trans = transform;
			trans.position += trans.rotation * Vector3.up * _lastWalkBob.z + trans.rotation * Vector3.left * _lastWalkBob.y * 1.25f;
			transform.rotation *= Quaternion.Euler( _lastWalkBob.z * 2, _lastWalkBob.y * 4, _lastWalkBob.x * 4 );
		}

		// Fields

		[SerializeField]
		private float bobBounceScale = 0.5f;

		[SerializeField]
		private float bobSpeedScale = 15f;
	}
}
