using UnityEngine;

namespace Espionage.Engine.Cameras
{
	public class ThirdPersonTripod : Behaviour, ITripod, IControls
	{
		public void Activated( ref ITripod.Setup camSetup )
		{
			camSetup.Position = Local.Pawn.EyePos;
			camSetup.Rotation = Local.Pawn.EyeRot;
		}

		public void Deactivated() { }

		private Vector3 _smoothedPosition;

		void ITripod.Build( ref ITripod.Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			var offseted = camSetup.Rotation * Vector3.left * offset.x + camSetup.Rotation * Vector3.up * offset.y + camSetup.Rotation * Vector3.forward * offset.z;
			_smoothedPosition = Vector3.Slerp( _smoothedPosition, Local.Pawn.EyePos + camSetup.Rotation * Vector3.back * distance + offseted, smoothing * Time.deltaTime );
			camSetup.Position = _smoothedPosition;

			// Offset

			camSetup.Rotation = Local.Pawn.EyeRot;
		}

		void IControls.Build( ref IControls.Setup setup )
		{
			setup.ViewAngles += new Vector3( -setup.MouseDelta.y, setup.MouseDelta.x, 0 );
			setup.ViewAngles.x = Mathf.Clamp( setup.ViewAngles.x, -pitchClamp.x, pitchClamp.y );
		}

		// Fields

		[SerializeField]
		private float smoothing = 15;

		[SerializeField]
		private float distance = 5;

		[SerializeField]
		private Vector2 pitchClamp = new( 88, 15 );

		[SerializeField]
		private Vector3 offset;
	}
}
