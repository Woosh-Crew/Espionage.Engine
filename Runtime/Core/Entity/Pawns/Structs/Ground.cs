using UnityEngine;

namespace Espionage.Engine
{
	public readonly struct Ground : ILibrary
	{
		public Library ClassInfo { get; }

		public static Ground Get( Vector3 position )
		{
			var hit = Physics.Raycast( position + Vector3.up * 0.05f, Vector3.down, out var info, 0.1f, ~LayerMask.GetMask( "Ignore Raycast", "Pawn" ), QueryTriggerInteraction.Ignore );
			return new( hit, info );
		}

		private Ground( bool landed, RaycastHit hit )
		{
			ClassInfo = Library.Database[typeof( Ground )];

			IsGrounded = landed;
			Hit = hit;
			Entity = null;
			Surface = null;

			if ( !landed )
			{
				// Don't grab if we didn't ground
				return;
			}

			// Get Entity
			if ( hit.collider.TryGetComponent<Entity>( out var ent ) )
			{
				Entity = ent;
			}

			// Get Surface
			if ( Entity != null )
			{
				Surface = Entity.Get<ISurface>().Surface;
			}

			// Still null, see if theres a mono behaviour
			if ( Surface == null && hit.collider.TryGetComponent<ISurface>( out var surface ) )
			{
				Surface = surface.Surface;
			}
		}

		public RaycastHit Hit { get; }

		public bool IsGrounded { get; }
		public Vector3 Normal => Hit.normal;

		public Entity Entity { get; }
		public Surface Surface { get; }
	}
}
