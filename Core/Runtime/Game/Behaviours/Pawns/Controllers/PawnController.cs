using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public abstract class PawnController : Component<Pawn>, ISimulated
	{
		public virtual void Simulate( Client client )
		{
			var input = client.Input;

			Entity.EyeRot = Quaternion.Euler( input.ViewAngles );
			Entity.EyePos = transform.position + Vector3.Scale( Vector3.up, Entity.transform.lossyScale ) * eyeHeight;

			transform.localRotation = Quaternion.AngleAxis( Entity.EyeRot.eulerAngles.y, Vector3.up );
		}

		// Fields

		[SerializeField]
		private float eyeHeight = 1.65f;
	}
}
