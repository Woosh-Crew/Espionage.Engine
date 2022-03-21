using Espionage.Engine.Tripods;

namespace Espionage.Engine
{
	public abstract partial class Game
	{
		[Function( "dev.tripod" ), Terminal]
		private static void DevTripodCommand()
		{
			if ( Local.Client.Tripod is DevTripod )
			{
				Local.Client.Tripod.Delete();
				Local.Client.Tripod = null;
				return;
			}

			Local.Client.Tripod = new DevTripod();
		}

		[Function( "dev.noclip" ), Terminal]
		private static void NoclipCommand()
		{
			Dev.Log.Info( "This would Noclip" );
		}
	}
}
