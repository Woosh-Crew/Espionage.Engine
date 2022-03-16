using Espionage.Engine.Tripods;

namespace Espionage.Engine
{
	public abstract partial class Game
	{
		//
		// Dev
		//

		[Function( "dev.tripod" ), Terminal]
		private static void DevTripodCMD()
		{
			Local.Client.Tripod = Local.Client.Tripod is DevTripod ? null : new DevTripod();
		}

		[Function( "dev.noclip" ), Terminal]
		private static void NoclipCMD()
		{
			Dev.Log.Info( "This would Noclip" );
		}
	}
}
