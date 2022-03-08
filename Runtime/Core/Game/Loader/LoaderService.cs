using UnityEngine.SceneManagement;

namespace Espionage.Engine.Services
{
	internal class LoaderService : Service
	{
		public Loader Loader => Engine.Game.Loader;
		private Scene _scene;

		public override void OnReady()
		{
			if ( Loader == null )
			{
				Debugging.Log.Warning( "No Loader found on Game" );
				return;
			}
		}
	}
}
