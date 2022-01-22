using UnityEngine;
using UnityEngine.SceneManagement;

namespace Espionage.Engine
{
	public static class SceneExtensions
	{
		/// <summary>
		/// Instantiate a GameObject and place it in this Scene.
		/// </summary>
		/// <param name="self">A Scene instance.</param>
		/// <param name="original">An existing object that you want to make a copy of.</param>
		/// <returns>The instantiated GameObject.</returns>
		public static GameObject Instantiate( this Scene self, GameObject original )
		{
			var o = Object.Instantiate( original );

			if ( !self.isLoaded || !self.IsValid() )
			{
				Debugging.Log.Warning( "Scene wasn't valid" );
				return o;
			}

			if ( o == null )
			{
				return null;
			}

			SceneManager.MoveGameObjectToScene( o, self );
			return o;
		}

		/// <summary>
		/// Unload this Scene.
		/// </summary>
		/// <param name="self">A Scene instance.</param>
		public static void Unload( this Scene self )
		{
			if ( self.isLoaded && self.IsValid() )
			{
				SceneManager.UnloadSceneAsync( self.name );
			}
		}

		/// <summary>
		/// Get a Component of Type T in this Scene. Returns the first found Component.
		/// </summary>
		/// <typeparam name="T">A Type that derives from Component</typeparam>
		/// <param name="self">A Scene instance.</param>
		/// <returns>A Component of Type T or null if none is found.</returns>
		public static T GetComponentInScene<T>( this Scene self )
		{
			if ( !self.isLoaded || !self.IsValid() )
			{
				return default;
			}

			foreach ( var go in self.GetRootGameObjects() )
			{
				var component = go.GetComponentInChildren<T>( true );
				if ( component != null )
				{
					return component;
				}
			}

			return default;
		}
	}
}
