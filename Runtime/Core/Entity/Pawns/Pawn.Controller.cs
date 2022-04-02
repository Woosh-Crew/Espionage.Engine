using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Espionage.Engine
{
	public partial class Pawn
	{
		public abstract class Controller : Component<Pawn>, IControls
		{
			public float EyeHeight { get; set; } = 1.65f;
			public bool Enabled { get; set; }

			public void Simulate( Client client )
			{
				Client = client;

				if ( !Enabled )
				{
					return;
				}

				Grab( Entity );
				Simulate();
				Finalise( Entity );
			}

			private void Grab( Pawn pawn )
			{
				Velocity = pawn.Velocity;

				Rotation = pawn.Rotation;
				Position = pawn.Position;

				EyePos = pawn.Eyes.Position;
				EyeRot = pawn.Eyes.Rotation;
			}

			private void Finalise( Pawn pawn )
			{
				pawn.Velocity = Velocity;
				pawn.transform.localRotation = Rotation;

				pawn.Eyes = new( EyePos, EyeRot );
			}

			// Pawn

			protected Client Client { get; set; }

			protected Quaternion Rotation { get; set; }
			protected Vector3 Position { get; private set; }

			protected Vector3 EyePos { get; set; }
			protected Quaternion EyeRot { get; set; }

			// Controller

			[SuppressMessage( "ReSharper", "InconsistentNaming" )]
			protected Vector3 Velocity;

			protected virtual void Simulate()
			{
				EyeRot = Quaternion.Euler( Client.Input.ViewAngles );
				EyePos = Position + Vector3.Scale( Vector3.up, Entity.Scale ) * EyeHeight;

				Rotation = Quaternion.AngleAxis( EyeRot.eulerAngles.y, Vector3.up );
			}

			// Input

			public virtual void Build( Controls.Setup setup ) { }
		}
	}
}
