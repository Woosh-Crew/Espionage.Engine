using System.IO;

namespace Espionage.Engine.Services
{
	[Order( 150 )] // Load after everything
	internal class ModService : Service
	{
		public override void OnReady()
		{
			// Cache all Mods
			foreach ( var modDir in Files.Pathing.All( "mods://", SearchOption.TopDirectoryOnly ) )
			{
				_ = new Mod( Files.Pathing.Name( modDir ), modDir );
			}
		}
	}
}
