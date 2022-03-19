using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public class NoclipController : Pawn.Controller
	{
		private Vector3 _targetPos;

		protected override void Simulate()
		{
			base.Simulate();

			var direction = new Vector2( Client.Input.Forward, Client.Input.Horizontal );

			if ( _targetPos == default )
			{
				_targetPos = Entity.transform.position;
			}

			// Movement

			var vel = Entity.EyeRot * Vector3.forward * direction.x + Entity.EyeRot * Vector3.right * direction.y;

			if ( Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.E ) )
			{
				vel += Vector3.up * 1;
			}

			if ( Input.GetKey( KeyCode.LeftControl ) || Input.GetKey( KeyCode.Q ) )
			{
				vel += Vector3.down * 1;
			}

			vel = vel.normalized * 20;

			if ( Input.GetKey( KeyCode.LeftShift ) )
			{
				vel *= 5.0f;
			}

			if ( Input.GetKey( KeyCode.LeftAlt ) )
			{
				vel *= 0.2f;
			}

			_targetPos += vel * Time.deltaTime;

			Velocity = vel;

			Entity.transform.position = Vector3.Lerp( Entity.transform.position, _targetPos, 5 * Time.deltaTime );
		}
	}
}
