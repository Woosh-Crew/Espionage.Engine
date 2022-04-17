using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// A Floor is a struct containing data that is
	/// below a pawn. This includes the surface its standing
	/// on, the normal and if it is grounded.
	/// </summary>
	public readonly struct Floor : ILibrary
	{
		public Library ClassInfo => Library.Database[typeof( Floor )];

		public static Floor Get( Vector3 position, float offset = 0 )
		{
			var hit = Trace.Ray( position + (Vector3.down * offset) + (Vector3.up * 0.05f), Vector3.down, 0.1f )
				.Ignore( "Ignore Raycast", "Pawn" )
				.Radius( 0.1f )
				.Run( out var info );

			return new( hit, info );
		}

		private Floor( bool landed, Trace.Result hit )
		{
			IsGrounded = landed;
			Hit = hit;
			Surface = null;

			if ( !landed )
			{
				// Don't grab if we didn't ground
				return;
			}

			// Still null, see if theres a mono behaviour
			if ( Surface == null && hit.Collision.TryGetComponent<ISurface>( out var surface ) )
			{
				Surface = surface.Surface;
			}
		}

		public override string ToString()
		{
			return $"Normal: {Normal}, Floored: {IsGrounded}";
		}

		public Trace.Result Hit { get; }

		public bool IsGrounded { get; }
		public Vector3 Normal => Hit.Normal;

		public Surface Surface { get; }

		// Helpers

		public static implicit operator bool( Floor floor )
		{
			return floor.IsGrounded;
		}

		public static implicit operator Surface( Floor floor )
		{
			return floor.Surface;
		}
	}
}
