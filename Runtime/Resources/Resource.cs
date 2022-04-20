using Espionage.Engine.Services;

namespace Espionage.Engine.Resources
{
	public partial class Resource : Service
	{
		public override void OnReady()
		{
			base.OnReady();
			
			// Get reference pathing to all resources
			// Load default resources (Maps & Mods)
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			
			// Watch resource for change
			// Unload resource after 1 minute of inactive use
		}

		public override void OnShutdown()
		{
			base.OnShutdown();
			
			// Stop watching resources for updates
			// Unload any currently loaded resources
		}
	}
}
