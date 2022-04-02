using UnityEngine;

namespace Espionage.Engine
{
	public class Interactor : Component<Actor>, ISimulated
	{
		[Slider( 0.1f, 10 )]
		public float Distance { get; set; } = 2;

		// Logic

		public void Simulate( Client cl )
		{
			if ( !Input.GetKeyDown( KeyCode.E ) )
			{
				return;
			}

			var usable = Trace.Ray( Entity.EyePos, Entity.EyeRot * Vector3.forward, Distance )
				.Radius( 0.2f )
				.Ignore( "Pawn" )
				.Run<IUsable>();

			if ( usable != null && usable.CanUse( Entity ) )
			{
				usable.OnInteract( Entity );
			}
		}
	}
}
