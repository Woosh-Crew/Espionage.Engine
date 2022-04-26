using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Espionage.Engine
{
	public class Bootstrap
	{
		public Bootstrap( Action shutdown, Action update )
		{
			OnShutdown += shutdown;
			OnUpdate += update;
		}

		internal virtual void Inject()
		{
			Application.quitting += Shutdown;

			var loop = PlayerLoop.GetCurrentPlayerLoop();

			for ( var i = 0; i < loop.subSystemList.Length; ++i )
			{
				// Frame Update
				if ( loop.subSystemList[i].type == typeof( Update ) )
				{
					loop.subSystemList[i].updateDelegate += Update;
				}
			}

			PlayerLoop.SetPlayerLoop( loop );
		}

		internal virtual void Remove()
		{
			Application.quitting -= Shutdown;

			var loop = PlayerLoop.GetCurrentPlayerLoop();

			for ( var i = 0; i < loop.subSystemList.Length; ++i )
			{
				// Frame Update
				if ( loop.subSystemList[i].type == typeof( Update ) )
				{
					loop.subSystemList[i].updateDelegate -= Update;
				}
			}

			PlayerLoop.SetPlayerLoop( loop );
		}

		protected void Update()
		{
			OnUpdate?.Invoke();
		}

		protected void Shutdown()
		{
			OnShutdown?.Invoke();
		}

		public event Action OnUpdate;
		public event Action OnShutdown;
	}
}
