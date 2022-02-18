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

			_targetRot += input.ViewAngles;
			_targetRot.y = Mathf.Clamp( _targetRot.y, -88, 88 );

			Pawn.EyeRot = Quaternion.AngleAxis( _targetRot.x, Vector3.up ) * Quaternion.AngleAxis( _targetRot.y, Vector3.left );
			Pawn.EyePos = transform.position + Vector3.up * 1.65f;

			transform.localRotation = Quaternion.AngleAxis( _targetRot.x, Vector3.up );
		}

		private Vector2 _targetRot;
	}
}
