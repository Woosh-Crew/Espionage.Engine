using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public abstract class PawnController : Behaviour, IComponent<Pawn>
	{
		void IComponent<Pawn>.OnAttached( Pawn item ) { }

		private Vector2 _targetRot;

		public override void Simulate( Client client )
		{
			var input = client.Input;

			_targetRot += input.ViewAngles;
			_targetRot.y = Mathf.Clamp( _targetRot.y, -88, 88 );

			EyeRot = Quaternion.AngleAxis( _targetRot.x, Vector3.up ) * Quaternion.AngleAxis( _targetRot.y, Vector3.left );
			EyePos = Vector3.up * 1.65f;
		}


		public Vector3 EyePos { get; protected set; }
		public Quaternion EyeRot { get; protected set; }
	}
}
