using UnityEngine;

namespace Espionage.Engine
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
			setup.ViewAngles *= Quaternion.AngleAxis( setup.MouseDelta.x, Vector3.up ) * Quaternion.AngleAxis( setup.MouseDelta.y, Vector3.left );
		}
	}
}
