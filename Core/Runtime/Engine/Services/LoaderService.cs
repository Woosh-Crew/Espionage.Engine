using UnityEngine.SceneManagement;

namespace Espionage.Engine.Services
{
	public class LoaderService : IService
	{
		public Library ClassInfo { get; }

		public LoaderService()
		{
			ClassInfo = Library.Database[GetType()];
			Callback.Register( this );
		}

		~LoaderService()
		{
			Callback.Unregister( this );
		}

		public Loader Loader => Engine.Game.Loader;
		private Scene _scene;

		public void OnReady()
		{
			if ( Loader == null )
			{
				Debugging.Log.Warning( "No Loader found on Game" );
				return;
			}

			SceneManager.LoadScene( Loader.Scene, LoadSceneMode.Additive );
			_scene = SceneManager.GetSceneByPath( Loader.Scene );
		}

		// Not Needed

		public void OnShutdown() { }
		public void OnUpdate() { }
		public void Dispose() { }
	}
}
