namespace Espionage.Engine.Services.Camera
{
	public interface ICameraService : IService { }

	internal class CameraService : ICameraService
	{
		// Ready

		public void OnReady()
		{
			_camera = CameraController.Instance;
			Engine.AddToLayer( _camera.gameObject );
		}
		
		public void OnShutdown() { }
		
		// Frame

		private CameraController _camera;
		private Tripod.Setup _lastSetup;

		public void OnUpdate()
		{
			if ( Engine.Game == null )
				return;

			// Build the camSetup, from game.
			_lastSetup = Engine.Game.BuildCamera( _lastSetup );

			// Get Camera Component
			_camera.Finalise( _lastSetup );
		}
	}
}
