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

				Rotation = pawn.Rotation;
				Position = pawn.Position;

				EyePos = pawn.EyePos;
				EyeRot = pawn.EyeRot;
			}

			protected void Finalise( Pawn pawn )
			{
				pawn.Velocity = Velocity;
				pawn.Rotation = Rotation;

				pawn.EyePos = EyePos;
				pawn.EyeRot = EyeRot;
			}

			// Pawn

			protected Client Client { get; set; }

			protected Quaternion Rotation { get; set; }
			protected Vector3 Position { get; private set; }

			public Vector3 EyePos { get; set; }
			public Quaternion EyeRot { get; set; }

			// Controller

			public Vector3 Velocity { get; set; }

			protected virtual void Simulate()
			{
				EyeRot = Quaternion.Euler( Client.Input.ViewAngles );
				EyePos = Position + Vector3.Scale( Vector3.up, Entity.Scale ) * eyeHeight;

				Rotation = Quaternion.AngleAxis( EyeRot.eulerAngles.y, Vector3.up );
			}

			// Input

			public virtual void Build( IControls.Setup setup ) { }

			// Fields

			[SerializeField]
			private float eyeHeight = 1.65f;
		}
	}
}
