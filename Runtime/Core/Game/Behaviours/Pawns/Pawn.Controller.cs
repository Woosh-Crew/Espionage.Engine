using UnityEngine;

namespace Espionage.Engine
{
	public partial class Pawn
	{
		public abstract class Controller : Component<Pawn>, ISimulated, IControls
		{
			public void Simulate( Client client )
			{
				Client = client;
				
				Grab( Entity );
				Simulate();
				Finalise( Entity );
			}

			protected void Grab( Pawn pawn )
			{
				Velocity = pawn.Velocity;
			}

			protected void Finalise( Pawn pawn )
			{
				pawn.Velocity = Velocity;
			}
			
			// Client
			
			protected Client Client { get; set; }

			// Controller

			protected Vector3 Velocity { get; set; }

			protected abstract void Simulate();

			// Input

			public virtual void Build( IControls.Setup setup ) { }
		}
	}
}
