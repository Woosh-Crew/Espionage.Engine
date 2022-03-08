using System;
using UnityEngine;

namespace Espionage.Engine.Pickups.Viewmodels
{
	public class BobAnimation : Viewmodel.Effect
	{
		private float _walkBobDelta;
		private float _walkBobScale;
		private float _dampedSpeed;
		private Vector3 _lastWalkBob;

		public override void PostCameraSetup( ref ITripod.Setup camSetup )
		{
			var velocity = Local.Pawn.Velocity;
			var speed = Mathf.InverseLerp( 0, 15, velocity.magnitude );

			_dampedSpeed = Mathf.Lerp( _dampedSpeed, speed, 2 * Time.deltaTime );
			_walkBobScale = Mathf.Lerp( _walkBobScale, speed, 10 * Time.deltaTime );
			_walkBobDelta += Time.deltaTime * bobSpeedScale * _walkBobScale;

			// Waves
			_lastWalkBob.x = MathF.Sin( _walkBobDelta * 0.7f ) * 0.6f;
			_lastWalkBob.y = MathF.Cos( _walkBobDelta * 0.7f ) * 0.4f;
			_lastWalkBob.z = MathF.Cos( _walkBobDelta * 1.3f ) * 0.8f;

			// Scale walk bob off property
			_lastWalkBob *= _dampedSpeed * bobBounceScale * Time.deltaTime;

			var trans = transform;

			trans.rotation *= Quaternion.Euler( _lastWalkBob.z * 2, _lastWalkBob.y * 4, _lastWalkBob.x * 4 );
			trans.position += trans.rotation * Vector3.up * _lastWalkBob.z + trans.rotation * Vector3.left * _lastWalkBob.y * 1.25f;
		}

		// Fields

		[SerializeField]
		private float bobBounceScale = 0.5f;

		[SerializeField]
		private float bobSpeedScale = 15f;
	}
}
