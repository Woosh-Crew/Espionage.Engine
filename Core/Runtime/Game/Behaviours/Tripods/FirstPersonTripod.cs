using UnityEngine;

namespace Espionage.Engine.Cameras
{
	public class FirstPersonTripod : Behaviour, ITripod, IControls
	{
		public void Activated( ref ITripod.Setup camSetup )
		{
			camSetup.Position = Local.Pawn.EyePos;
			camSetup.Rotation = Local.Pawn.EyeRot;
		}

		public void Deactivated() { }

		void ITripod.Build( ref ITripod.Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			camSetup.Position = Local.Pawn.EyePos;
			camSetup.Rotation = Local.Pawn.EyeRot;
			camSetup.Viewer = Local.Pawn.gameObject;
		}

		void IControls.Build( ref IControls.Setup setup )
		{
			setup.ViewAngles += new Vector3( -setup.MouseDelta.y, setup.MouseDelta.x, 0 );
			setup.ViewAngles.x = Mathf.Clamp( setup.ViewAngles.x, -pitchClamp, pitchClamp );
		}

		// Fields

		[SerializeField]
		private float pitchClamp = 88;
	}
}
