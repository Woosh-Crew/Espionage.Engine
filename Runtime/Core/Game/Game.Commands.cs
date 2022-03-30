using Espionage.Engine.Tripods;

namespace Espionage.Engine
{
	public abstract partial class Game
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
	}
}
