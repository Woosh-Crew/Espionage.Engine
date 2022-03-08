using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public class NoclipController : Pawn.IController
	{
		private Vector3 _targetPos;
		
		public void Simulate( Client cl, Pawn pawn )
		{
			var direction = new Vector2( cl.Input.Forward, cl.Input.Horizontal );

			if ( _targetPos == default )	
			{
				_targetPos = pawn.transform.position;
			}
			
			// Movement

			var vel = pawn.EyeRot * Vector3.forward * direction.x + pawn.EyeRot * Vector3.right * direction.y;

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

			pawn.transform.position = Vector3.Lerp( pawn.transform.position, _targetPos, 5 * Time.deltaTime );
		}
	}
}
