using System;
using UnityEngine;

namespace Espionage.Engine.Pickups.Viewmodels
{
	public class BobAnimation : Behaviour, Viewmodel.IEffect
	{
		private float _walkBobDelta;
		private float _walkBobScale;
		private float _dampedSpeed;
		private Vector3 _lastWalkBob;

		public void PostCameraSetup( ref ITripod.Setup camSetup )
		{
			var speed = Owner.Velocity.Length.LerpInverse( 0, 240 );
			
			_dampedSpeed = _dampedSpeed.LerpTo( speed, 2 * Time.Delta );
			_walkBobScale = _walkBobScale.LerpTo( Local.Pawn.GroundEntity != null ? speed : 0, 10 * Time.Delta );
			_walkBobDelta += Time.deltaTime * 15.0f * _walkBobScale;

			// Waves
			_lastWalkBob.x = MathF.Sin( _walkBobDelta * 0.7f ) * 0.6f;
			_lastWalkBob.y = MathF.Cos( _walkBobDelta * 0.7f ) * 0.4f;
			_lastWalkBob.z = MathF.Cos( _walkBobDelta * 1.3f ) * 0.8f;

			// Scale walk bob off property
			_lastWalkBob *= _dampedSpeed;

			Position += Rotation.Up * _lastWalkBob.z+ Rotation.Left * _lastWalkBob.y * 1.25f;
			Rotation *= Rotation.From( _lastWalkBob.z * 2, _lastWalkBob.y * 4, _lastWalkBob.x * 4 );
		}
	}
}
