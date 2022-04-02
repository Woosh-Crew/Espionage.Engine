using UnityEngine;

namespace Espionage.Engine
{
	public class Leaning : Component<Actor>, ISimulated, Pawn.ICallbacks
	{
		public float Distance { get; set; } = 0.25f;
		public float Angle { get; set; } = 15;
		
		// Logic
		
		protected int State { get; private set; }

		public void Simulate( Client cl )
		{
			// Left Lean
			if ( Input.GetKeyDown( KeyCode.Q ) )
			{
				Lean( -1 );
			}

			// Right lean
			if ( Input.GetKeyDown( KeyCode.E ) )
			{
				Lean( 1 );
			}

			// If Jump return 
			if ( !Entity.Ground && State != 0 )
			{
				State = 0;
			}
		}

		public virtual void Lean( int state )
		{
			if ( !Entity.Ground )
			{
				Dev.Log.Info("ground");
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
		}

		protected virtual float GetLeanDistance( float leanState )
		{
			const float girth = 1f;

			var hit = Trace.Ray( Entity.EyePos, Entity.EyeRot.Right() * leanState, girth )
				.Ignore( "Pawn" )
				.Radius( 0.2f )
				.Run( out var tr );

			return hit ? tr.Value.distance / girth : 1;
		}
	}
}
