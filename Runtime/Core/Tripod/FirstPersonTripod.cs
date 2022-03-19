using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine.Tripods
{
	[Library( "tripod.first_person" )]
	public class FirstPersonTripod : Tripod
	{
		public override void Activated( ref Setup camSetup )
		{
			camSetup.Rotation = Local.Pawn.EyeRot;
			camSetup.Position = Local.Pawn.EyePos;
		}

		protected override void OnBuildTripod( ref Setup camSetup )
		{
			if ( Local.Pawn == null )
			{
				return;
			}

			camSetup.Viewer = Entity.Visuals;

			camSetup.Rotation = Local.Pawn.EyeRot;
			camSetup.Position = Local.Pawn.EyePos;
		}
	}
}
