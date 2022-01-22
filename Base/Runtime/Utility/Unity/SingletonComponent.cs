using UnityEngine;

namespace Espionage.Engine
{
	public class SingletonComponent<T> : MonoBehaviour  where T : Component
	{
		private static T _instance;

		/// <summary>
		/// Singleton instance.
		/// </summary>
		public static T Instance
		{
			get
			{
				// If we already have instance, return it
				if ( _instance != null )
				{
					return _instance;
				}
				
				// Find the instance
				_instance = FindObjectOfType<T>();
				if ( _instance != null )
				{
					return _instance;
				}

				var obj = new GameObject { name = typeof(T).Name };
				_instance = obj.AddComponent<T>();

				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this as T;
				DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		protected virtual void OnDestroy()
		{
			if(_instance == this)
				_instance = null;
		}
	}
}
