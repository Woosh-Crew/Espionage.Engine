using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public abstract class PawnController : Behaviour, IComponent<Pawn>
	{
		void IComponent<Pawn>.OnAttached( Pawn item )
		{
			Pawn = item;
		}

		public Pawn Pawn { get; private set; }

		public override void Simulate( Client client )
		{
			var input = client.Input;

			Pawn.EyeRot = input.ViewAngles;
			Pawn.EyePos = transform.position + Vector3.up * 1.65f;

			transform.localRotation = Quaternion.AngleAxis( Pawn.EyeRot.eulerAngles.y, Vector3.up );
		}
	}
}
