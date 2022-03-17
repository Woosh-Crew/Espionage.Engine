using Espionage.Engine.Tripods;

namespace Espionage.Engine
{
	public abstract partial class Game
	{
		[Function( "dev.tripod" ), Terminal]
		private static void DevTripodCommand()
		{
			Local.Client.Tripod = Local.Client.Tripod is DevTripod ? null : new DevTripod();
		}

		[Function( "dev.noclip" ), Terminal]
		private static void NoclipCommand()
		{
			Dev.Log.Info( "This would Noclip" );
		}
	}
}
