using UnityEngine;

namespace Espionage.Engine
{
	public class Interactor : Component<Actor>, ISimulated
	{
		public float Distance { get; set; } = 2;

		// Logic

		public void Simulate( Client cl )
		{
			if ( Input.GetKeyDown( KeyCode.E ) )
			{
				// Fire Ray
				var ray = Physics.Raycast( Entity.EyePos, Entity.EyeRot * Vector3.forward, out var result, Distance, ~LayerMask.GetMask( "Pawn" ) );

				if ( ray )
				{
					if ( result.collider.TryGetComponent<IUsable>( out var usable ) && usable.CanUse( Entity ) )
					{
						usable.OnInteract( Entity );
					}
				}
			}
		}
	}
}
