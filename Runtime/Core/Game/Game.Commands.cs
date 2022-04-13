using Espionage.Engine.Tripods;

namespace Espionage.Engine
{
	public partial class Game
	{
		[Function( "dev.tripod" ), Terminal]
		private static void DevTripodCommand()
		{
			if ( Local.Client.Tripod.Is<DevTripod>() )
			{
				Local.Client.Tripod.Pop();
				return;
			}

			Local.Client.Tripod.Push<DevTripod>();
		}

		[Function( "dev.noclip" ), Terminal]
		private static void DevNoclipCommand()
		{
			var pawn = Local.Client.Pawn;

			if ( pawn.DevController == null )
			{
				pawn.DevController = pawn.Components.Create<NoclipController>();
			}
			else
			{
				var controller = pawn.Components.Get<NoclipController>();

				pawn.Components.Remove( controller );
				pawn.DevController = null;

				controller.Delete();
			}
		}
	}
}
