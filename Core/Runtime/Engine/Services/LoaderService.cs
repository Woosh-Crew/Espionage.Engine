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
			SceneManager.LoadScene( Loader.ScenePath, LoadSceneMode.Additive );
			_scene = SceneManager.GetSceneByPath( Loader.ScenePath );
		}

		// Callbacks

		[Callback( "map.loading" )]
		private void MapLoad() { }

		// Not Needed

		public void OnShutdown() { }

		public void OnUpdate() { }
	}
}
