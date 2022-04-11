using UnityEngine;

namespace Espionage.Engine
{
	[Singleton]
	public class Leaning : Component<Actor>, ISimulated, Pawn.ICallbacks
	{
		public float Distance { get; set; } = 0.25f;
		public float Angle { get; set; } = 15;

		// Logic

		protected int State { get; private set; }

		public void Simulate( Client cl )
		{
			// Left Lean
			if ( Controls.Scheme["Lean.Left"].Pressed )
			{
				Lean( -1 );
			}

			// Right lean
			if ( Controls.Scheme["Lean.Right"].Pressed )
			{
				Lean( 1 );
			}

			// If Jump return 
			if ( !Entity.Floor && State != 0 )
			{
				State = 0;
			}
		}

		public virtual void Lean( int state )
		{
			if ( !Entity.Floor )
			{
				return;
			}

			if ( State == state )
			{
				State = 0;
				return;
			}

			State += state;
			State = Mathf.Clamp( State, -1, 1 );
		}

		// Camera

		private float _leanStateDamped;
		private float _distance;

		public void PostCameraSetup( ref Tripod.Setup setup )
		{
			// Get Distance
			_distance = GetLeanDistance( State );

			// Apply Distance
			_leanStateDamped = _leanStateDamped.LerpTo( State * _distance, 5 * Time.deltaTime );

			setup.Rotation *= Quaternion.Euler( 0, 0, -Angle * _leanStateDamped );
			setup.Position += setup.Rotation.Right() * Distance * _leanStateDamped;

			setup.Viewmodel.Angles *= Quaternion.Euler( 0, 0, -Angle / 2 * _leanStateDamped );

			setup.Viewmodel.Offset += setup.Rotation.Right() * (_leanStateDamped * 0.25f * Distance);
			setup.Viewmodel.Offset += setup.Rotation.Down() * (_leanStateDamped * 0.02f * Distance);
		}

		protected virtual float GetLeanDistance( float leanState )
		{
			const float girth = 1f;

			var hit = Trace.Ray( Entity.Eyes.Position, Entity.Eyes.Rotation.Right() * leanState, girth )
				.Ignore( "Pawn" )
				.Radius( 0.2f )
				.Run( out var tr );

			return hit ? tr.Value.distance / girth : 1;
		}
	}
}
